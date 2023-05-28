using System;
using System.Runtime.CompilerServices;

namespace ExPlugins.EXtensions;

public abstract class ErrorReporter
{
	protected string ErrorLimitMessage;

	protected int MaxErrors = 5;

	protected Action OnErrorLimitReached;

	[CompilerGenerated]
	private int int_0;

	[CompilerGenerated]
	private bool bool_0;

	public int ErrorCount
	{
		[CompilerGenerated]
		get
		{
			return int_0;
		}
		[CompilerGenerated]
		private set
		{
			int_0 = value;
		}
	}

	public bool ErrorLimitReached
	{
		[CompilerGenerated]
		get
		{
			return bool_0;
		}
		[CompilerGenerated]
		private set
		{
			bool_0 = value;
		}
	}

	public void ReportError()
	{
		int errorCount = ErrorCount + 1;
		ErrorCount = errorCount;
		GlobalLog.Error($"[{GetType().Name}] Error count: {ErrorCount}/{MaxErrors}");
		if (ErrorCount == MaxErrors)
		{
			ErrorLimitReached = true;
			if (ErrorLimitMessage != null)
			{
				GlobalLog.Error(ErrorLimitMessage);
			}
			OnErrorLimitReached?.Invoke();
		}
	}

	public void ResetErrors()
	{
		ErrorCount = 0;
		ErrorLimitReached = false;
	}
}
