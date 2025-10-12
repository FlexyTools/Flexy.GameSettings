namespace Flexy.GameSettings
{
	public class Service_GameSettings : MonoBehaviour
	{
		public static SerializerImpl _serializerImpl;
		public static SerializerImpl Serializer => _serializerImpl ??= new();
		
		private readonly List<GameSettingsTab> _settings = new();
		
		public	T		Get<T>		( ) where T:GameSettingsTab, new() 
		{
			foreach (var s in _settings)
			{
				if (s is T typedS)
					return typedS;
			}
			
			var result = new T();
			_settings.Add(result);
			
			return result;
		}
		
		[ContextMenu("Reset To Defaults")]
		public	void	ResetToDefaults	( )	
		{
			foreach (var ss in _settings) 
				ss.SetDefault();
		}

		public class  SerializerImpl
		{
			public 			Boolean	GetBool		( String key, Boolean defaultValue )=> GetInt	(key, defaultValue ? 1 : 0) != 0;
			public 			void	SetBool		( String key, Boolean value )		=> SetInt	(key, value ? 1 : 0);
			
			public virtual	Int32	GetInt		( String key, Int32 defaultValue )	=> PlayerPrefs.GetInt		(key, defaultValue);
			public virtual	void	SetInt		( String key, Int32 value )			=> PlayerPrefs.SetInt		(key, value);
			
			public virtual	Single	GetFloat	( String key, Single defaultValue )	=> PlayerPrefs.GetFloat		(key, defaultValue);
			public virtual	void	SetFloat	( String key, Single value )		=> PlayerPrefs.SetFloat		(key, value);
			
			public virtual	String	GetString	( String key, String defaultValue )	=> PlayerPrefs.GetString	(key, defaultValue);
			public virtual	void	SetString	( String key, String value )		=> PlayerPrefs.SetString	(key, value);

			public virtual	Boolean HasKey		( String key )						=> PlayerPrefs.HasKey		(key);
			public virtual	void	ClearKey	( String key )						=> PlayerPrefs.DeleteKey	(key);
		}
		
		#if UNITY_EDITOR
		[UnityEditor.MenuItem("Tools/Fun.Flexy/Game Settings/Reset To Defaults")]
		private static	void	Editor_ResetToDefaults	( )		
		{
			var service = FindAnyObjectByType<Service_GameSettings>();
			service.ResetToDefaults();
		}
		[UnityEditor.MenuItem("Tools/Fun.Flexy/Game Settings/Clear All Prefs")]
		private static	void	Editor_ClearAllPrefs	( )		
		{
			PlayerPrefs.DeleteAll();
		}
		#endif
	}
	
	public abstract class GameSettingsTab
	{
		public void SetDefault ( )
		{
			foreach (var field in GetType().GetFields())
			{
				var settingVal = field.GetValue(this);  
				switch (settingVal)
				{
					case BooleanSetting s:	s.SetDefault(); break;
					case Int32Setting s:	s.SetDefault(); break;
					case SingleSetting s:	s.SetDefault(); break;
					case StringSetting s:	s.SetDefault(); break;
					case ColorSetting s:	s.SetDefault(); break;
					case IClearable s:		s.SetDefault(); break;
				}
			}
		}
	}
	
	public interface IClearable{ public void SetDefault( ); }
}