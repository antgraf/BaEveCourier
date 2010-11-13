using ExecutionActors;

namespace Courier
{
	public interface ISettingsProvider
	{
		Settings GetSettings();
		void SaveSettings();
	}
}
