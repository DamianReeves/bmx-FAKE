using System;
using System.Web.UI.WebControls;
using Inedo.BuildMaster.Extensibility.Actions;
using Inedo.BuildMaster.Web.Controls;
using Inedo.BuildMaster.Web.Controls.Extensions;
using Inedo.Web.Controls;

namespace Inedo.BuildMasterExtensions.Fake
{
    internal sealed class FakeActionEditor : ActionEditorBase
    {
        private SourceControlFileFolderPicker _txtFakeExecutablePath;
        private SourceControlFileFolderPicker _txtWorkingDirectory;
        private ValidatingTextBox _txtFakeFile;
        private ValidatingTextBox _txtTasks;
        private ValidatingTextBox _txtVariableValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeActionEditor"/> class.
        /// </summary>
        public FakeActionEditor()
        {
        }

        public override void BindToForm(ActionBase extension)
        {
            var fakeAction = (FakeAction)extension;

            this._txtFakeExecutablePath.Text = fakeAction.FakeExecutablePath;
            this._txtWorkingDirectory.Text = fakeAction.WorkingDirectory;
            this._txtFakeFile.Text = fakeAction.FakeFile;
            this._txtTasks.Text = fakeAction.Tasks;
            this._txtVariableValues.Text = string.Join(Environment.NewLine, fakeAction.VariableValues ?? new string[0]);
        }

        public override ActionBase CreateFromForm()
        {
            return new FakeAction
            {
                FakeExecutablePath = this._txtFakeExecutablePath.Text,
                WorkingDirectory = this._txtWorkingDirectory.Text,
                FakeFile = this._txtFakeFile.Text,
                Tasks = this._txtTasks.Text,
                VariableValues = this._txtVariableValues.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries),
            };
        }

        protected override void CreateChildControls()
        {
            this._txtFakeExecutablePath = new SourceControlFileFolderPicker
            {
                ServerId = this.ServerId,
                Required = true
            };

            this._txtWorkingDirectory = new SourceControlFileFolderPicker
            {
                ServerId = this.ServerId,
                DefaultText = "default"
            };

            this._txtFakeFile = new ValidatingTextBox { Width = 300 };

            this._txtTasks = new ValidatingTextBox { Width = 300 };

            this._txtVariableValues = new ValidatingTextBox
            {
                TextMode = TextBoxMode.MultiLine,
                Rows = 5,
                Width = 300
            };

            this.Controls.Add(
                new FormFieldGroup("Fake Executable Path",
                    "The path to the Fake executable.",
                    false,
                    new StandardFormField("Fake Executable Path:", this._txtFakeExecutablePath),
                    new StandardFormField("Working Directory:", this._txtWorkingDirectory)
                ),
                new FormFieldGroup("Fake File",
                    "The optional Fake File to use, relative to the working directory.",
                    false,
                    new StandardFormField("Fake File:", this._txtFakeFile)
                ),
                new FormFieldGroup("Tasks",
                    "Enter the tasks to run, separated by spaces.",
                    false,
                    new StandardFormField("Build Properties:", this._txtTasks)
                ),
                new FormFieldGroup("Environment Variables",
                    "You may optionally specify additional environment variables and values for this execution, separated by newlines. For example:<br />opt1=value1<br />opt2=value2",
                    true,
                    new StandardFormField("Environment Variables:", this._txtVariableValues)
                )
            );
        }
    }
}
