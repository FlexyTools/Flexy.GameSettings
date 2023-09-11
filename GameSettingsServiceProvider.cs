
namespace Flexy.GameSettings
{
	public class GameSettingsServiceProvider : GameWorlds.ServiceProvider
	{
		public override void ProvideServices( GameWorlds.GameWorld world )
		{
			var settings = new GameSettings( );
			settings.Init( );
			
			world.SetService( settings );
		}
	}
}
