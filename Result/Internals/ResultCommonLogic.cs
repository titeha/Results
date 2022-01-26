using Result.Exceptions;

namespace Result.Internals
{
 internal static class ResultCommonLogic
 {
  internal static E GetErrorWithSuccessGuard<E>(bool isFailure, E error) => isFailure ? error : throw new ResultSuccessException();

  internal static bool ErrorStateGuard<E>(bool isFailure, E error)
  {
   if (isFailure)
   {
	if (error == null || error is string && error.Equals(string.Empty))
	 throw new ArgumentNullException();
   }
   else
   {
	if (!EqualityComparer<E>.Default.Equals(error, default))
	 throw new ArgumentException();
   }

   return isFailure;
  }
 }
}