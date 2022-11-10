using System;
using System.Collections.Generic;
using ProManager.Models;
namespace ProManager.Implementations
{
    public sealed class ProjectComparer : IComparer<Project>
    {
        internal static ProjectComparer SingleTone = new ProjectComparer();
        public int Compare(Project first, Project second)
        {
            var difference = (first.UpdateAt - second.UpdateAt).TotalMilliseconds;
            return Convert.ToInt32(Math.Floor(difference));
        }
    }
}
