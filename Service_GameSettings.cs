namespace Flexy.GameSettings
{
	public class Service_GameSettings : MonoBehaviour, IService
	{
		public static SerializerImpl _serializerImpl;
		public static SerializerImpl Serializer => _serializerImpl ??= new( );
		
		private readonly List<GameSettingsTab> _settings = new( );
		
		public void OrderedInit(GameContext ctx) { }
		
		public T Get<T>( ) where T:GameSettingsTab, new() 
		{
			foreach (var s in _settings)
			{
				if( s is T typedS )
					return typedS;
			}
			
			var result = new T();
			_settings.Add( result );
			
			return result;
		}
		
		[ContextMenu("Restore Defaults")]
		public 			void		ClearGameSettings				(  )
		{
			foreach ( var ss in _settings ) 
				ss.Clear( );
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
			public virtual	void	ClearKey	( String key )						=> PlayerPrefs.DeleteKey	( key );
		}
	}
	
	public abstract class GameSettingsTab
	{
		public void Clear ( )
		{
			foreach (var field in GetType().GetFields())
			{
				var settingVal = field.GetValue(this);  
				switch (settingVal)
				{
					case BooleanSetting s:	s.Clear( ); break;
					case Int32Setting s:	s.Clear( ); break;
					case SingleSetting s:	s.Clear( ); break;
					case StringSetting s:	s.Clear( ); break;
					case IClearable s:		s.Clear( ); break;
				}
			}
		}
	}
}