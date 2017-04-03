using System;
using System.Collections.Generic;
using System.Text;

namespace repack {
    interface IObserver {
        void OnProgress(String message);
    }

}
