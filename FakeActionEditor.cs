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
        private SourceControlFileFolderPicker txtFakeExecutablePath;
        private SourceControlFileFolderPicker txtWorkingDirectory;
        private ValidatingTextBox txtFakeFile;
        private ValidatingTextBox txtTasks;
        private ValidatingTextBox txtVariableValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeActionEditor"/> class.
        /// </summary>
        public FakeActionEditor()
        {
        }

        public override void BindToForm(ActionBase extension)
        {
            var fakeAction = (FakeAction)extension;

            this.txtFakeExecutablePath.Text = fakeAction.FakeExecutablePath;
            this.txtWorkingDirectory.Text = fakeAction.WorkingDirectory;
            this.txtFakeFile.Text = fakeAction.FakeFile;
            this.txtTasks.Text = fakeAction.Tasks;
            this.txtVariableValues.Text = string.Join(Environment.NewLine, fakeAction.VariableValues ?? new string[0]);
        }

        public override ActionBase CreateFromForm()
        {
            return new FakeAction
            {
                FakeExecutablePath = this.txtFakeExecutablePath.Text,
                WorkingDirectory = this.txtWorkingDirectory.Text,
                FakeFile = this.txtFakeFile.Text,
                Tasks = this.txtTasks.Text,
                VariableValues = this.txtVariableValues.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries),
            };
        }

        protected override void CreateChildControls()
        {
            this.txtFakeExecutablePath = new SourceControlFileFolderPicker
            {
                ServerId = this.ServerId,
                Required = true
            };

            this.txtWorkingDirectory = new SourceControlFileFolderPicker
            {
                ServerId = this.ServerId,
                DefaultText = "default"
            };

            this.txtFakeFile = new ValidatingTextBox { Width = 300 };

            this.txtTasks = new ValidatingTextBox { Width = 300 };

            this.txtVariableValues = new ValidatingTextBox
            {
                TextMode = TextBoxMode.MultiLine,
                Rows = 5,
                Width = 300
            };

            this.Controls.Add(
                new FormFieldGroup("Fake Executable Path",
                    "The path to the Fake executable.",
                    false,
                    new StandardFormField("Fake Executable Path:", this.txtFakeExecutablePath),
                    new StandardFormField("Working Directory:", this.txtWorkingDirectory)
                ),
                new FormFieldGroup("Fake File",
                    "The optional Fake File to use, relative to the working directory.",
                    false,
                    new StandardFormField("Fake File:", this.txtFakeFile)
                ),
                new FormFieldGroup("Tasks",
                    "Enter the tasks to run, separated by spaces.",
                    false,
                    new StandardFormField("Build Properties:", this.txtTasks)
                ),
                new FormFieldGroup("Environment Variables",
                    "You may optionally specify additional environment variables and values for this execution, separated by newlines. For example:<br />opt1=value1<br />opt2=value2",
                    true,
                    new StandardFormField("Environment Variables:", this.txtVariableValues)
                )
            );
        }
    }
}
