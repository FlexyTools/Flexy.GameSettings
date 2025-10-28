namespace Flexy.GameSettings;

public struct BooleanSetting
{
	public	BooleanSetting	( String key, Boolean defaultValue, Boolean readLater = false )
	{
		_key		= "Flexy.GameSettings  Boolean " + key;
		_default	= defaultValue;
		_value		= readLater ? _default : SGS.Serializer.GetBool(_key, defaultValue);
		Changed		= null;
	}

	private readonly	String		_key;
	private readonly	Boolean		_default; 
	private				Boolean		_value; 
	
	public event Action<Boolean>?	Changed;

	public	Boolean		HasValue	=> SGS.Serializer.HasKey(_key);

	public	Boolean		Read		( ) => _value = SGS.Serializer.GetBool(_key, _default);
	public	Boolean		Get			( ) => _value;
	public	void		Set			( Boolean value )
	{ 
		if (_value == value )
			return;

		_value = value;
		SGS.Serializer.SetBool(_key, value);

		try						{ Changed?.Invoke(value); }
		catch (Exception ex)	{ Debug.LogException(ex); }
	}
	public	void		SetDefault	( ) => Set(_default);

	public static	implicit operator Boolean ( BooleanSetting @this ) => @this._value;
}