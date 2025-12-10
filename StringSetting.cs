namespace Flexy.GameSettings;

public struct StringSetting
{
	public StringSetting ( String key, String defaultValue, Boolean readLater = false )
	{
		_key     = key;
		_default = defaultValue;
		_value   = readLater ? _default : SGS.Store.GetString(_key, _default);
		Changed  = null;
	}

	private readonly 	String	_key;
	private readonly 	String	_default;
	private				String	_value; 
	
	public event Action<String>? Changed;

	public	Boolean	HasValue	=> SGS.Store.HasKey(_key);

	public	String	Read		( ) => _value = SGS.Store.GetString(_key, _default);
	public	String	Get			( )
	{ 
		return _value;
	}
	public	void	Set			( String value )
	{ 
		if (_value == value)
			return;
		
		_value = value;
		SGS.Store.SetString(_key, value);

		try						{ Changed?.Invoke(value); }
		catch (Exception ex)	{ Debug.LogException(ex); }
	}
	public	void	SetDefault	( ) => Set(_default);

	public static	implicit operator String ( StringSetting @this ) => @this._value;
}