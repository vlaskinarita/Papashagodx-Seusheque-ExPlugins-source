using System.Threading.Tasks;
using ExPlugins.EXtensions;
using ExPlugins.EXtensions.CachedObjects;

internal abstract class Class36 : ErrorReporter
{
	protected static readonly ExtensionsSettings Settings;

	public abstract bool Enabled { get; }

	public abstract bool ShouldExecute { get; }

	protected Class36()
	{
		ErrorLimitMessage = "Too many errors. Now disabling this vendoring module until combat area change.";
		OnErrorLimitReached = ResetData;
	}

	public abstract Task Execute();

	public abstract void OnStashing(CachedItem item);

	public abstract void ResetData();

	static Class36()
	{
		Settings = ExtensionsSettings.Instance;
	}
}
