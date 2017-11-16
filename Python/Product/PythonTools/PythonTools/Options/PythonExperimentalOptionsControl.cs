// Python Tools for Visual Studio
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
using System.Windows.Forms;

namespace Microsoft.PythonTools.Options {
    public partial class PythonExperimentalOptionsControl : UserControl {
        public PythonExperimentalOptionsControl() {
            InitializeComponent();
        }

        internal void SyncControlWithPageSettings(PythonToolsService pyService) {
            _noDatabaseFactory.Checked = pyService.ExperimentalOptions.NoDatabaseFactory;
            _condaEnvironments.Checked = pyService.ExperimentalOptions.AutoDetectCondaEnvironments;
            _condaPackageManager.Checked = pyService.ExperimentalOptions.UseCondaPackageManager;
            _useNewPtvsDebugger.Checked = pyService.ExperimentalOptions.UseNewPtvsDebugger;
        }

        internal void SyncPageWithControlSettings(PythonToolsService pyService) {
            pyService.ExperimentalOptions.NoDatabaseFactory = _noDatabaseFactory.Checked;
            pyService.ExperimentalOptions.AutoDetectCondaEnvironments = _condaEnvironments.Checked;
            pyService.ExperimentalOptions.UseCondaPackageManager = _condaPackageManager.Checked;
            pyService.ExperimentalOptions.UseNewPtvsDebugger = _useNewPtvsDebugger.Checked;
        }
    }
}
