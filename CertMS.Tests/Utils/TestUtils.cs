using System;
using System.Threading;
using CertMS.CertificateGenerator;
using Xunit;

namespace CertMS.Tests.Utils
{
	internal static class TestUtils
	{
		internal static void NotNull(object obj)
		{
			Assert.NotNull(obj);
		}

		internal static void AreEqual<T>(T expected, T actual)
		{
			Assert.Equal(expected, actual);
		}

		internal static void AreNotEqual<T>(T expected, T actual)
		{
			Assert.NotEqual(expected, actual);
		}

		internal static void True(bool? condition)
		{
			Assert.True(condition);
		}

		internal static void ThrowsInvalidCertificateDatesException(Action function)
		{
			ThrowsException<InvalidCertificateDates>(function);
		}

		internal static void ThrowsArgumentException(Action function)
		{
			ThrowsException<ArgumentException>(function);
		}

		private static void ThrowsException<T>(Action function) where T : Exception
		{
			Assert.Throws<T>(function);
		}

		internal static void RunInStaThread(ThreadStart funcToExecute)
		{
			var staThread = new Thread(funcToExecute);
			staThread.SetApartmentState(ApartmentState.STA);
			staThread.Start();
			staThread.Join();
		}
	}
}
