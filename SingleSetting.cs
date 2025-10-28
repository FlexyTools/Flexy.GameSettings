namespace Flexy.GameSettings;

public struct SingleSetting
{
	public SingleSetting ( String key, Single defaultValue, Boolean readLater = false )
	{
		_key     = key;
		_default = defaultValue;
		_value   = readLater ? _default : SGS.Serializer.GetFloat(_key, _default);
		Changed  = null;
	}

	private readonly	String	_key;
	private	readonly	Single	_default;
	private				Single	_value; 
	
	public event Action<Single>? Changed;

	public	Boolean	HasValue	=> SGS.Serializer.HasKey(_key);

	public	Single	Read		( ) => _value = SGS.Serializer.GetFloat(_key, _default);
	public	Single	Get			( )
	{ 
		return _value;
	}
	public	void	Set			( Single value )
	{ 
		if (_value == value)
			return;

		_value = value;
		SGS.Serializer.SetFloat(_key, value);
		
		try						{ Changed?.Invoke(value); }
		catch (Exception ex)	{ Debug.LogException(ex); }
	}
	public	void	SetDefault	( ) => Set(_default);

	public static	implicit operator Single ( SingleSetting @this ) => @this._value;
}