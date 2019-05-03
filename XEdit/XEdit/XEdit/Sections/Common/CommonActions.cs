using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XEdit.Sections
{
    public static class CommonActions
    {
        public static async Task DefaultCommit()
        {
            await UniqueInstancesManager.Get<ImageWorker>().CommitImage();
        }
    }
}
