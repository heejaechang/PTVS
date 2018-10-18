﻿// Python Tools for Visual Studio
// Copyright(c) Microsoft Corporation
// All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the License); you may not use
// this file except in compliance with the License. You may obtain a copy of the
// License at http://www.apache.org/licenses/LICENSE-2.0
//
// THIS CODE IS PROVIDED ON AN  *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS
// OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY
// IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
// MERCHANTABLITY OR NON-INFRINGEMENT.
//
// See the Apache Version 2.0 License for specific language governing
// permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.PythonTools.Interpreter {
    [Export(typeof(IPackageManagerProvider))]
    sealed class CPythonCondaPackageManagerProvider : IPackageManagerProvider {
        private readonly IServiceProvider _site;
        private Lazy<string> _latestCondaExe;

        [ImportingConstructor]
        public CPythonCondaPackageManagerProvider(
            [Import(typeof(SVsServiceProvider), AllowDefault = true)] IServiceProvider site = null
        ) {
            _site = site;

            // This can be slow, if there are 2 or more global conda installations
            // (some conda versions have long startup time), so we only fetch it once.
            _latestCondaExe = new Lazy<string>(() => CondaUtils.GetRootCondaExecutablePath(_site));
        }

        public IEnumerable<IPackageManager> GetPackageManagers(IPythonInterpreterFactory factory) {
            var prefixPath = factory.Configuration.PrefixPath;
            if (string.IsNullOrEmpty(prefixPath) ||
                !Directory.Exists(Path.Combine(prefixPath, "conda-meta"))) {
                yield break;
            }

            var condaPath = CondaUtils.GetCondaExecutablePath(prefixPath);
            if (string.IsNullOrEmpty(condaPath)) {
                // conda.bat is no longer present in a conda 4.4 environment,
                // so find a global conda.exe to use.
                condaPath = _latestCondaExe.Value;
            }

            if (string.IsNullOrEmpty(condaPath)) {
                yield break;
            }

            IPackageManager pm = null;
            try {
                pm = new CondaPackageManager(factory, condaPath);
            } catch (NotSupportedException) {
                pm = null;
            }
            if (pm != null) {
                yield return pm;
            }
        }
    }
}
