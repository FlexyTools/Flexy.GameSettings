namespace Flexy.GameSettings;

public		struct EnumSetting<T> : IClearable where T: unmanaged, Enum, IComparable
{
	public EnumSetting ( String key, T defaultValue )
	{
		var isInt64 = Enum.GetUnderlyingType(typeof(T)) == typeof(Int64);

		_key        = $"Flexy.GameSettings  Enum {typeof(T).Name} " + key;
		_default    = defaultValue;
		Changed		= null;
		
		if (isInt64)
		{
			var raw		= EnumUnion.TtoV64( defaultValue );
			_value      = EnumUnion.VtoT<T>(	
							(Int64)SGS.Serializer.GetInt(_key+"_Low", (Int32)raw) |
							(Int64)SGS.Serializer.GetInt(_key+"_High", (Int32)(raw>>32) ) << 32
						);
		}
		else
		{
			_value      = EnumUnion.VtoT<T>( SGS.Serializer.GetInt(_key, EnumUnion.TtoV( defaultValue )) );
		}
	}

	private	String	_key;
	private	T		_default;
	private	T		_value;
	
	public	Boolean	HasValue => SGS.Serializer.HasKey( _key );

	public event	Action<T> Changed;
	
	public T Get	( )
	{ 
		return _value;
	}
	public void Set	( T value )
	{ 
		if ( value.CompareTo( _value ) == 0 )
			return;

		_value = value;
		var isInt64 = Enum.GetUnderlyingType(typeof(T)) == typeof(Int64);
		
		if (isInt64)
		{
			var raw		= EnumUnion.TtoV64( value );
			SGS.Serializer.SetInt(_key+"_Low", (Int32)raw);
			SGS.Serializer.SetInt(_key+"_High", (Int32)(raw>>32) );
		}
		else
		{
			SGS.Serializer.SetInt( _key, EnumUnion.TtoV(value) );
		}

		try						{ Changed?.Invoke( value ); }
		catch (Exception ex)	{ Debug.LogException( ex ); }
	}
	public void Clear() => Set( _default );

	public static	implicit operator T ( EnumSetting<T> @this ) => @this._value;
}
public interface IClearable{ public void Clear( ); }