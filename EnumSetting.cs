using Flexy.Utils;

namespace Flexy.GameSettings;

public		struct EnumSetting<T> : IClearable where T: unmanaged, Enum, IComparable
{
	public EnumSetting ( String key, T defaultValue )
	{
		_key        = $"Flexy.GameSettings  Enum {typeof(T).Name} " + key;
		_default    = defaultValue;
		_value      = EnumUnion.VtoT<T>( GSS.Serializer.GetInt(_key, EnumUnion.TtoV( defaultValue )) );
		Changed		= null;
	}

	private	String	_key;
	private	T		_default;
	private	T		_value;
	
	public	Boolean	HasValue => GSS.Serializer.HasKey( _key );

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
		GSS.Serializer.SetInt( _key, EnumUnion.TtoV(value) );

		try						{ Changed?.Invoke( value ); }
		catch (Exception ex)	{ Debug.LogException( ex ); }
	}
	public void Clear() => Set( _default );

	public static	implicit operator T ( EnumSetting<T> @this ) => @this._value;
}
public interface IClearable{ public void Clear( ); }