namespace Flexy.GameSettings
{
	public class GameSettings
	{
		[Header("QualityOptions")]
		public List<RenderQualityOptions> QualityOptions;
		
		public void Init( )
		{
			ToggleFps60		.Changed	+= i => ApplyApplicationChange( );
			
			MaxLodLevel		.Changed	+= i => ApplyQualityChange( );
			TextureQuality	.Changed	+= i => ApplyQualityChange( );
			AnisotropicFiltering.Changed	+= i => ApplyQualityChange( );
			RenderScale		.Changed	+= i => ApplyQualityChange( );
			Msaa			.Changed	+= i => ApplyQualityChange( );
		}

		public static SerializerImpl _serializerImpl;
		public static SerializerImpl Serializer => _serializerImpl ??= new SerializerImpl( );
		
		//Forbidden to call Unity from field initializer
		//Game locale - its not device locale, but locale from Flexy Localization
		//[Category("Text")]
		//public EnumSetting<SystemLanguage>	Locale				= new ( "Locale", Application.systemLanguage ); 

		// [Category("Input")]
		// public SingleSetting		FistPersonSensivity		= new ( "FPSensivity"			, 0.5f );
		// public SingleSetting		ThirdPersonSensivity	= new ( "TPSensivity"			, 0.5f );
		// public BooleanSetting	DeviceVibration			= new ( "DeviceVibration"		, true );
		
		[Category("Graphics Settings")]
		public Int32Setting					MaxLodLevel			= new ( "MaxLodLevel",0);
		public Int32Setting					DefaultFov			= new ( "DefaultFov", 60);
		public Int32Setting					Msaa				= new ( "Msaa_v4",2); //2 (adreno), 4 (others) mostly free. bigger values create small overhead default 2
		public EnumSetting<ETextureLimit>	TextureQuality		= new ( "TextureQuality", ETextureLimit.FullRes );
		public EnumSetting<AnisotropicFiltering> AnisotropicFiltering =	new( "AnisotropicFiltering", UnityEngine.AnisotropicFiltering.Enable );
		public BooleanSetting				UseHDR				= new ( "UseHDR", true);					
		public EnumSetting<ERenderScale>	RenderScale			= new ( "RenderScale", ERenderScale.Full );					
		public BooleanSetting				SrpBatcher			= new ( "SrpBatcher", false);					
		public BooleanSetting				DynamicBatcher		= new ( "DynamicBatcher", false);					
		public BooleanSetting				PostProcessing		= new ( "PostProcessing", false );
		public BooleanSetting				DevelopMode			= new ( "DevelopMode", false );
		
		
		[Category("Display")]
		public BooleanSetting				ShowFps				= new ( "ShowFps", false );
		public BooleanSetting				ToggleFps60			= new ( "ToggleFps60", false );

		[Category("Audio")]
		public SingleSetting				MusicVolume			= new ( "MusicVolume", 1 );
		public SingleSetting				SfxVolume		    = new ( "SfxVolume", 1 );
		
		private void						ApplyQualityChange			( )	
		{
			// Лимит текстур для стриминга установить в 0,25 от обьема памяти устройства
			//QualitySettings.streamingMipmapsMemoryBudget = SystemInfo.systemMemorySize / 4f;
			
			// подсчет dpi
			// var targetHeight = 1080;
			// switch ( RenderScale.Get(  ) )
			// {
			// 	case ERenderScale.Low:		targetHeight = 540; break;
			// 	case ERenderScale.Medium:	targetHeight = 720; break;
			// 	case ERenderScale.Full:		targetHeight = 1080; break;
			// }
		
			// RET THIS
			//
			// if( targetHeight < Display.main.systemHeight )
			// 	Screen.SetResolution( (Int32)( Display.main.systemWidth * (targetHeight / (Single)Display.main.systemHeight ) ), targetHeight, false );
			//
			// INSTEAD OF 
			//
			// QualitySettings.resolutionScalingFixedDPIFactor = Screen.dpi / 1000;
			// QualitySettings.resolutionScalingFixedDPIFactor *= Math.Min( Display.main.systemHeight, targetHeight) / (Single)Display.main.systemHeight;
			//
			// QualitySettings.antiAliasing		= Msaa; //2 (adreno), 4 (others) mostly free. bigger values create small overhead default 4
			//((UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset).msaaSampleCount = Msaa;

			//QualitySettings.maximumLODLevel		= MaxLodLevel;
			
			QualitySettings.globalTextureMipmapLimit	= Mathf.Max((Int32) MaxTextureLimit(),(Int32)TextureQuality.Get(  ));
			QualitySettings.anisotropicFiltering		= AnisotropicFiltering.Get(  );
		}
		private void						ApplyApplicationChange		( )	
		{
			//Application.targetFrameRate = ToggleFps60 ? 60 : 30 ;
		}
		
		private ETextureLimit				MaxTextureLimit				( ) 
		{
			var memory = Mathf.RoundToInt(SystemInfo.systemMemorySize);

			if (memory	>=	6)
				return ETextureLimit.FullRes;
			if (memory	>=	4)
				return ETextureLimit.HalfRes;
			if (memory	>=	3)
				return ETextureLimit.Quarter;

			return ETextureLimit.Eighth;
		}
		
		[Serializable]
		public struct RenderQualityOptions
		{
			public Int32 Option_01;
			public Int32 Option_02;
			public Int32 Option_03;
		}

		public class  SerializerImpl
		{
			public 			Boolean	GetBool		( String key, Boolean defaultValue )=> GetInt	( key, defaultValue ? 1 : 0 ) != 0;
			public 			void	SetBool		( String key, Boolean value )		=> SetInt	( key, value ? 1 : 0 );
			
			public virtual	Int32	GetInt		( String key, Int32 defaultValue )	=> PlayerPrefs.GetInt		( key, defaultValue );
			public virtual	void	SetInt		( String key, Int32 value )			=> PlayerPrefs.SetInt		( key, value );
			
			public virtual	Single	GetFloat	( String key, Single defaultValue )	=> PlayerPrefs.GetFloat		( key, defaultValue );
			public virtual	void	SetFloat	( String key, Single value )		=> PlayerPrefs.SetFloat		( key, value );
			
			public virtual	String	GetString	( String key, String defaultValue )	=> PlayerPrefs.GetString	( key, defaultValue );
			public virtual	void	SetString	( String key, String value )		=> PlayerPrefs.SetString	( key, value );

			public virtual	Boolean HasKey		( String key )						=> PlayerPrefs.HasKey		( key );			
		}
		
		public class CategoryAttribute : HeaderAttribute
		{
			public CategoryAttribute(String header) : base(header) { }
		}

	#if UNITY_EDITOR
		[UnityEditor.MenuItem("FlexyGame/Clear/ClearGameSettings")]
		public static			void		ClearGameSettings				(  )
		{ 
			PlayerPrefs.DeleteKey("RQManualChanged" );
			PlayerPrefs.DeleteKey("RenderQuality" );
			PlayerPrefs.DeleteKey("MoveStickType"		);
			PlayerPrefs.DeleteKey("MoveStickSmooth"		);
			PlayerPrefs.DeleteKey("MoveStickDeadZone"	);
			PlayerPrefs.DeleteKey("TPSensivity"			);
			PlayerPrefs.DeleteKey("FPSensivity"			);
			PlayerPrefs.DeleteKey("AutoSnap"			);
			PlayerPrefs.DeleteKey("ZoomControl"			);
			PlayerPrefs.DeleteKey("HudPositions"		);
			PlayerPrefs.DeleteKey("SceneDetailsSettings"		);
			PlayerPrefs.DeleteKey(nameof(ToggleFps60));
		}
	#endif

	}

	public enum ERenderScale : Byte	
	{
		Full			= 0,
		ThreeQuarters	= 1,
		Half			= 2,
		Quarter			= 3,
	}

	public enum ETextureLimit : Byte	
	{
		FullRes		= 0,
		HalfRes		= 1,
		Quarter		= 2,
		Eighth		= 3,
	}
}