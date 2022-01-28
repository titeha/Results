using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ResultType
{
 internal static class TaskExtensions
 {
  public static ConfiguredTaskAwaitable DefaultAwait(this Task task) => task.ConfigureAwait(Result.DefaultConfigureAwait);

  public static ConfiguredTaskAwaitable<T> DefaultAwait<T>(this Task<T> task) => task.ConfigureAwait(Result.DefaultConfigureAwait);
 }
}