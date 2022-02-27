using System.Runtime.CompilerServices;

namespace ResultType
{
 internal static class TaskExtensions
 {
  public static ConfiguredTaskAwaitable DefaultAwait(this Task task) => task.ConfigureAwait(Result.DefaultConfigureAwait);

  public static ConfiguredTaskAwaitable<T> DefaultAwait<T>(this Task<T> task) => task.ConfigureAwait(Result.DefaultConfigureAwait);
 }
}