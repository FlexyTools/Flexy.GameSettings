namespace Flexy.GameSettings;

public struct StringSetting
{
	public StringSetting (String key, String defaultValue)
	{
		_key     = key;
		_default = defaultValue;
		_value   = GameSettingsService.Serializer.GetString(_key, _default);
		Changed  = null;
	}

	private String _key;
	private String _default;
	private String _value; 
	
	public event Action<String> Changed;

	public Boolean HasValue => GameSettingsService.Serializer.HasKey( _key );

	public String Get	( )
	{ 
		return _value;
	}
	public void Set	( String value )
	{ 
		if(_value == value)
			return;
		
		_value = value;
		GameSettingsService.Serializer.SetString(_key, value);

		try						{ Changed?.Invoke( value ); }
		catch (Exception ex)	{ Debug.LogException( ex ); }
	}

	public static	implicit operator String ( StringSetting @this ) => @this._value;
}