namespace devdeer.DoctorFlox.Logic.Wpf.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides extension methods around the type <see cref="List{T}" />.
    /// </summary>
    public static class ListExtensions
    {
        #region methods

        /// <summary>
        /// Takes a bunch of <paramref name="lists" /> and merges them into one list.
        /// </summary>
        /// <typeparam name="T">The type of elements inside each list.</typeparam>
        /// <param name="lists">The bunch of lists.</param>
        /// <returns>The merged list.</returns>
        public static List<T> Merge<T>(this List<IEnumerable<T>> lists)
        {
            var result = new List<T>(lists.Sum(l => l.Count()));
            foreach (var list in lists)
            {
                result.AddRange(list);
            }
            return result;
        }

        #endregion
    }
}