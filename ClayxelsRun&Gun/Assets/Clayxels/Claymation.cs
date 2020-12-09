﻿
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Reflection;
#endif


namespace Clayxels{
	/* This component is used to display the point-cloud cache files baked by a ClayContainer.
	The Claymation component does not use heavy gpu computing so it's the ideal way to deploy point clouds on lower-end hardware.
	You can add this component to any GameObject and use it independently from ClayContainers.
	It uses the same shaders used by ClayContainer for live clayxels.
	*/
	[ExecuteInEditMode]
	public class Claymation : MonoBehaviour{
		/* This is a .clay.bytes asset generated by a ClayContainer using clayContainer.freezeClaymation().*/
		public TextAsset claymationFile = null;

		/* */
		public Claymation instanceOf = null;

		/* Use the same materials you use for a ClayContainer.*/
		public Material material = null;

		/* How fast will this animation play.*/
		public int frameRate = 30;

		/* If the claymationFile contains multiple frames this allows to play them automatically while the game runs.*/
		public bool playAnim = false;

		bool loaded = false;
		int fileFormat = 0;
		float voxelSize = 0.1f;
		float splatRadius = 0.1f;
		Bounds renderBounds = new Bounds();
		Vector3 boundsScale;
		int numFrames = 0;
		int numChunks = 0;
		int chunksX = 0;
		int chunksY = 0;
		int chunksZ = 0;
		int chunkSize = 0;
		int maxPointCount = 0;
		int[] numPointsAtFrame;
		List<uint[]> pointCloudData;
		List<uint[]> pointToChunkData;
		int[] chunkDataPointers;
		MaterialPropertyBlock materialProperties = null;
		ComputeBuffer pointCloudDataBuffer = null;
		ComputeBuffer pointToChunkIdBuffer = null;
		ComputeBuffer chunksCenterBuffer = null;
		Vector3[] chunksCenter;
		int frame = 0;
		float deltaTime = 0.0f;
		string renderPipe = "";
		bool initialized = false;
		Claymation _instance = null;

		static bool globalInit = false;

	    void Start(){
	    	this.init();
	    }

	     void OnDestroy(){
	     	this.reset();
	    }

	    public void init(){
	    	#if UNITY_EDITOR
				if(!Application.isPlaying && !Claymation.globalInit){
					Claymation.globalInit = true;
					this.reinstallEditorEvents();
				}
			#endif

	    	this.reset();

	    	this.initialized = true;

	    	if(this.instanceOf != null){
	    		this._instance = instanceOf;
	    		return;
	    	}

	    	if(this.claymationFile == null){
	    		return;
	    	}
	    	
	    	this._instance = this;

	    	this._instance.materialProperties = new MaterialPropertyBlock();
	    	
	    	string renderPipeAsset = "";
			if(GraphicsSettings.renderPipelineAsset != null){
				renderPipeAsset = GraphicsSettings.renderPipelineAsset.GetType().Name;
			}
			
			if(renderPipeAsset == "HDRenderPipelineAsset"){
				this.renderPipe = "hdrp";
			}
			else if(renderPipeAsset == "UniversalRenderPipelineAsset"){
				this.renderPipe = "urp";
			}
			else{
				this.renderPipe = "builtin";
			}

			this.loadClaymationFile();
	    }

	    void loadClaymationFile(){
	    	if(this.claymationFile == null){
	    		this.loaded = false;
	    		return;
	    	}

	    	MemoryStream stream = new MemoryStream(this.claymationFile.bytes);
	    	BinaryReader reader = new BinaryReader(stream);

	        this.fileFormat = reader.ReadInt32();
	        this.chunkSize = reader.ReadInt32();
	        this.chunksX = reader.ReadInt32();
	        this.chunksY = reader.ReadInt32();
	        this.chunksZ = reader.ReadInt32();
	        this.numFrames = reader.ReadInt32();

	        this.numChunks = this.chunksX * this.chunksY * this.chunksZ;
	        
	        this.numPointsAtFrame = new int[this.numFrames];
	        this.chunkDataPointers = new int[this.numFrames];

	        this.maxPointCount = 0;

	        this.pointCloudData = new List<uint[]>(this.numFrames);
	        this.pointToChunkData = new List<uint[]>(this.numFrames);
			
	        for(int frameIt = 0; frameIt < this.numFrames; ++frameIt){
				int numPoints = reader.ReadInt32();
				
				this.numPointsAtFrame[frameIt] = numPoints;

				if(numPoints > this.maxPointCount){
					this.maxPointCount = numPoints;
				}

				this.pointCloudData.Add(new uint[numPoints * 2]);
				this.pointToChunkData.Add(new uint[numPoints / 5]);

				for(int pointIt = 0; pointIt < numPoints; ++pointIt){
					this.pointCloudData[frameIt][pointIt * 2] = reader.ReadUInt32();
					this.pointCloudData[frameIt][(pointIt * 2) + 1] = reader.ReadUInt32();
				}

				for(int pointIt = 0; pointIt < numPoints / 5; ++pointIt){
					this.pointToChunkData[frameIt][pointIt] = reader.ReadUInt32();
				}
	        }
			
	        reader.Close();

	        if(this.material == null){
	        	if(this.renderPipe == "hdrp"){
					this.material = new Material(Shader.Find("Clayxels/ClayxelHDRPShader"));
				}
				else if(this.renderPipe == "urp"){
					this.material = new Material(Shader.Find("Clayxels/ClayxelURPShader"));
				}
				else{
					this.material = new Material(Shader.Find("Clayxels/ClayxelBuiltInShader"));
				}

	        	Texture texture = this.material.GetTexture("_MainTex");
				if(texture == null){
					this.material.SetTexture("_MainTex", (Texture)Resources.Load("clayxelDot"));
				}
			}

	        this.boundsScale = new Vector3(
	        	(float)this.chunkSize * this.chunksX,
				(float)this.chunkSize * this.chunksY,
				(float)this.chunkSize * this.chunksZ);

			float seamOffset = this.chunkSize / 256.0f; 
			float chunkOffset = this.chunkSize - seamOffset;
			float gridCenterOffset = (this.chunkSize * 0.5f);

			this.chunksCenter = new Vector3[this.numChunks];

			this.pointCloudDataBuffer = new ComputeBuffer(this.maxPointCount, sizeof(int) * 2);
			this.pointToChunkIdBuffer = new ComputeBuffer(this.maxPointCount / 5, sizeof(int));
			this.chunksCenterBuffer = new ComputeBuffer(this.numChunks, sizeof(float) * 3);

			int chunkIt = 0;
	        for(int z = 0; z < this.chunksZ; ++z){
				for(int y = 0; y < this.chunksY; ++y){
					for(int x = 0; x < this.chunksX; ++x){
						this.chunksCenter[chunkIt] = new Vector3(
							(-((this.chunkSize * this.chunksX) * 0.5f) + gridCenterOffset) + (chunkOffset * x),
							(-((this.chunkSize * this.chunksY) * 0.5f) + gridCenterOffset) + (chunkOffset * y),
							(-((this.chunkSize * this.chunksZ) * 0.5f) + gridCenterOffset) + (chunkOffset * z));
			        	
			        	chunkIt += 1;
			        }
			    }
	        }

	        this.chunksCenterBuffer.SetData(this.chunksCenter);

	        this.voxelSize = ((float)this.chunkSize / 256);

	        this.loadFrame(0);

	        this.loaded = true;
	    }

	    public int getFrame(){
	    	return this.frame;
	    }

	    public int getNumFrames(){
	    	return this.numFrames;
	    }

	    public void loadFrame(int frame){
	    	this.frame = frame;

	    	if(this.frame < 0){
	    		this.frame = 0;
	    	}
	    	else if(this.frame > this.numFrames - 1){
	    		this.frame = this.numFrames - 1;
	    	}
			
			int numPoints = this.numPointsAtFrame[this.frame];
	    	this.pointCloudDataBuffer.SetData(this.pointCloudData[this.frame], 0, 0, numPoints * 2);
	    	this.pointToChunkIdBuffer.SetData(this.pointToChunkData[this.frame], 0, 0, numPoints / 5);
	    }

	    #if UNITY_EDITOR
	    void Awake(){
	    	if(!Application.isPlaying){
	    		this.init();
	    	}
	    }

	    void reinstallEditorEvents(){
	    	AssemblyReloadEvents.beforeAssemblyReload -= this.onBeforeAssemblyReload;
	    	AssemblyReloadEvents.beforeAssemblyReload += this.onBeforeAssemblyReload;
	    }

	    void onBeforeAssemblyReload(){
	    	this.reset();
	    }

	    #endif

	    void reset(){
	    	this.initialized = false;

	    	if(this.pointCloudDataBuffer != null){
	    		this.pointCloudDataBuffer.Release();
	    	}

	    	if(this.pointToChunkIdBuffer != null){
	    		this.pointToChunkIdBuffer.Release();
	    	}

	    	if(this.chunksCenterBuffer != null){
	    		this.chunksCenterBuffer.Release();
	    	}

	    	this.pointCloudDataBuffer = null;
	    	this.pointToChunkIdBuffer = null;
	    	this.chunksCenterBuffer = null;

	    	this.numChunks = 0;
	    	this.loaded = false;
	    	this._instance = null;
	    }

	    void Update(){
	    	#if UNITY_EDITOR
	    		// reload claymation file in case unity discarded it in-editor
		    	if(!Application.isPlaying){
		    		if(!this.initialized){
		    			if(this.claymationFile != null || this.instanceOf != null){
		    				this.init();
		    			}
			    	}
			    }
		    #endif
			
	    	if(this._instance == null){
	    		return;
	    	}

	    	if(!this._instance.loaded){
	    		return;
	    	}

	    	if(this._instance.materialProperties == null){
	    		return;
	    	}

	    	if(!this._instance.enabled){
	    		return;
	    	}

			if(this.playAnim && this._instance == this){
				if(this.numFrames > 1){
					this.deltaTime += Time.deltaTime;

					if(this.deltaTime > 1.0f / this.frameRate){
						this.frame = (this.frame + 1) % this.numFrames;
				    	this.loadFrame(this.frame);
				    	
						this.deltaTime = 0.0f;
					}
				}
			}
			
			this.renderBounds.center = this.transform.position;
	    	this.renderBounds.size = this._instance.boundsScale * this.transform.lossyScale.x;

	    	this.splatRadius = this._instance.voxelSize * ((this.transform.lossyScale.x + this.transform.lossyScale.y + this.transform.lossyScale.z) / 3.0f);

    		int numPoints = this._instance.numPointsAtFrame[this.frame];
			
    		this._instance.materialProperties.SetMatrix("objectMatrix", this.transform.localToWorldMatrix);
			this._instance.materialProperties.SetFloat("splatRadius", this.splatRadius);
			this._instance.materialProperties.SetFloat("chunkSize", (float)this._instance.chunkSize);
			this._instance.materialProperties.SetBuffer("chunkPoints", this._instance.pointCloudDataBuffer);
			this._instance.materialProperties.SetBuffer("pointToChunkId", this._instance.pointToChunkIdBuffer);
			this._instance.materialProperties.SetBuffer("chunksCenter", this._instance.chunksCenterBuffer);
    		
			Graphics.DrawProcedural(
				this._instance.material, 
				this.renderBounds,
				MeshTopology.Triangles, numPoints * 3, 0,
				null, this._instance.materialProperties,
				ShadowCastingMode.On, true, this.gameObject.layer);
	    }
	}
}