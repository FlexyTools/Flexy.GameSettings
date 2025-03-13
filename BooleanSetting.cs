namespace Flexy.GameSettings;

public struct BooleanSetting
{
	public BooleanSetting (String key, Boolean defaultValue)
	{
		_key     = "Flexy.GameSettings  Boolean " + key;
		_default = defaultValue;
		_value   = GSS.Serializer.GetBool(_key, defaultValue );
		Changed  = null;
	}

	private String  _key;
	private Boolean _default; 
	private Boolean _value; 
	
	public event Action<Boolean> Changed;

	public Boolean HasValue => GSS.Serializer.HasKey( _key );

	public Boolean	Get		( ) => _value;

	public void		Set		( Boolean value )
	{ 
		if (_value == value )
			return;

		_value = value;
		GSS.Serializer.SetBool(_key, value );

		try						{ Changed?.Invoke( value ); }
		catch (Exception ex)	{ Debug.LogException( ex ); }
	}
	public void		Clear	( ) => Set( _default );

	public static	implicit operator Boolean ( BooleanSetting @this ) => @this._value;
}