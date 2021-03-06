﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelBaseFacts.editableobject.cs" company="Catel development team">
//   Copyright (c) 2008 - 2014 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Catel.Test.Data
{
    using System;
    using System.ComponentModel;
    using Catel.Data;

#if NETFX_CORE
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

    public partial class ModelBaseFacts
    {
        public class EditableObject : ModelBase
        {
            public EditableObject()
            {
                var advancedEditableObject = (IAdvancedEditableObject)this;
                advancedEditableObject.BeginEditing += (sender, e) => BeginEditingCalled = true;
                advancedEditableObject.CancelEditing += (sender, e) => CancelEditingCalled = true;
                advancedEditableObject.EndEditing += (sender, e) => EndEditingCalled = true;
            }

            public bool OnBeginEditCalled
            {
                get { return _onBeginEditCalled; }
                private set { _onBeginEditCalled = value; }
            }

            public bool BeginEditingCalled
            {
                get { return _beginEditingCalled; }
                private set { _beginEditingCalled = value; }
            }

            protected override void OnBeginEdit(BeginEditEventArgs e)
            {
                OnBeginEditCalled = true;
            }

            public bool OnCancelEditCalled
            {
                get { return _onCancelEditCalled; }
                private set { _onCancelEditCalled = value; }
            }

            public bool CancelEditingCalled
            {
                get { return _cancelEditingCalled; }
                private set { _cancelEditingCalled = value; }
            }

            protected override void OnCancelEdit(EditEventArgs e)
            {
                OnCancelEditCalled = true;
            }

            public bool OnEndEditCalled
            {
                get { return _onEndEditCalled; }
                private set { _onEndEditCalled = value; }
            }

            public bool EndEditingCalled
            {
                get { return _endEditingCalled; }
                private set { _endEditingCalled = value; }
            }

            protected override void OnEndEdit(EditEventArgs e)
            {
                OnEndEditCalled = true;
            }

            /// <summary>
            /// Gets or sets the ignored property.
            /// </summary>
            public int IgnoredPropertyInBackup
            {
                get { return GetValue<int>(IgnoredPropertyInBackupProperty); }
                set { SetValue(IgnoredPropertyInBackupProperty, value); }
            }

            /// <summary>
            /// Register the IgnoredPropertyInBackup property so it is known in the class.
            /// </summary>
            public static readonly PropertyData IgnoredPropertyInBackupProperty = RegisterProperty("IgnoredPropertyInBackup", typeof(int), 42,
                null, true, false);

            private bool _onBeginEditCalled;
            private bool _beginEditingCalled;
            private bool _onEndEditCalled;
            private bool _endEditingCalled;
            private bool _onCancelEditCalled;
            private bool _cancelEditingCalled;
        }

        public class ClearIsDirtyModel : ModelBase
        {
            public string Name
            {
                get { return GetValue<string>(NameProperty); }
                set { SetValue(NameProperty, value); }
            }

            public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string), default(string));

            public ClearIsDirtyModel()
            {

            }

            internal void ClearIsDirty()
            {
                IsDirty = false;
            }
        }

        [TestClass]
        public class TheBeginEditMethod
        {
            [TestMethod]
            public void AllowsDoubleCalls()
            {
                var editableObject = new EditableObject();
                var editableObjectAsIEditableObject = (IEditableObject)editableObject;

                editableObjectAsIEditableObject.BeginEdit();
                editableObjectAsIEditableObject.BeginEdit();
            }

            [TestMethod]
            public void InvokesBeginEditingEvent()
            {
                var editableObject = new EditableObject();
                var editableObjectAsIEditableObject = (IEditableObject)editableObject;

                Assert.IsFalse(editableObject.BeginEditingCalled);
                Assert.IsFalse(editableObject.OnBeginEditCalled);

                editableObjectAsIEditableObject.BeginEdit();

                Assert.IsTrue(editableObject.BeginEditingCalled);
                Assert.IsTrue(editableObject.OnBeginEditCalled);
            }
        }

        [TestClass]
        public class TheCancelEditMethod
        {
            [TestMethod]
            public void CancelsChangesCorrectlyForSimpleTypes()
            {
                var iniEntry = ModelBaseTestHelper.CreateIniEntryObject();
                var iniEntryAsIEditableObject = (IEditableObject)iniEntry;

                iniEntry.Value = "MyOldValue";

                iniEntryAsIEditableObject.BeginEdit();

                iniEntry.Value = "MyNewValue";

                iniEntryAsIEditableObject.CancelEdit();

                Assert.AreEqual("MyOldValue", iniEntry.Value);
            }

            [TestMethod]
            public void CancelsChangesCorrectlyForObjectWithCustomType()
            {
                var obj = new ObjectWithCustomType();
                var objEntryAsIEditableObject = (IEditableObject)obj;

                obj.Gender = Gender.Female;

                objEntryAsIEditableObject.BeginEdit();

                obj.Gender = Gender.Male;

                objEntryAsIEditableObject.CancelEdit();

                Assert.AreEqual(Gender.Female, obj.Gender);
            }

            [TestMethod]
            public void CancelsChangesForSelfReferencingTypes()
            {
                //Assert.Inconclusive("Fix in 3.1");
            }

            [TestMethod]
            public void DoesNotInvokeCancelEditingEventAfterBeginEditIsCalled()
            {
                var editableObject = new EditableObject();
                var editableObjectAsIEditableObject = (IEditableObject)editableObject;

                Assert.IsFalse(editableObject.CancelEditingCalled);
                Assert.IsFalse(editableObject.OnCancelEditCalled);

                editableObjectAsIEditableObject.CancelEdit();

                Assert.IsFalse(editableObject.CancelEditingCalled);
                Assert.IsFalse(editableObject.OnCancelEditCalled);
            }

            [TestMethod]
            public void InvokesCancelEditingEventAfterBeginEditIsCalled()
            {
                var editableObject = new EditableObject();
                var editableObjectAsIEditableObject = (IEditableObject)editableObject;

                Assert.IsFalse(editableObject.CancelEditingCalled);
                Assert.IsFalse(editableObject.OnCancelEditCalled);

                editableObjectAsIEditableObject.BeginEdit();
                editableObjectAsIEditableObject.CancelEdit();

                Assert.IsTrue(editableObject.CancelEditingCalled);
                Assert.IsTrue(editableObject.OnCancelEditCalled);
            }

            [TestMethod]
            public void IgnoresPropertiesNotInBackup()
            {
                var editableObject = new EditableObject();
                var editableObjectAsIEditableObject = (IEditableObject)editableObject;

                editableObject.IgnoredPropertyInBackup = 1;

                editableObjectAsIEditableObject.BeginEdit();

                editableObject.IgnoredPropertyInBackup = 2;

                editableObjectAsIEditableObject.CancelEdit();

                Assert.AreEqual(2, editableObject.IgnoredPropertyInBackup);
            }
        }

        [TestClass]
        public class TheEndEditMethod
        {
            [TestMethod]
            public void AppliesChangesCorrectlyForSimpleTypes()
            {
                var iniEntry = ModelBaseTestHelper.CreateIniEntryObject();
                var iniEntryAsIEditableObject = (IEditableObject)iniEntry;

                iniEntry.Value = "MyOldValue";

                iniEntryAsIEditableObject.BeginEdit();

                iniEntry.Value = "MyNewValue";

                iniEntryAsIEditableObject.EndEdit();

                Assert.AreEqual("MyNewValue", iniEntry.Value);
            }

            [TestMethod]
            public void AppliesChangesCorrectlyForObjectWithCustomType()
            {
                var obj = new ObjectWithCustomType();
                var objAsIEditableObject = (IEditableObject)obj;

                obj.Gender = Gender.Female;

                objAsIEditableObject.BeginEdit();

                obj.Gender = Gender.Male;

                ((IEditableObject)obj).EndEdit();

                Assert.AreEqual(Gender.Male, obj.Gender);
            }

            [TestMethod]
            public void AppliesChangesForSelfReferencingTypes()
            {
                //Assert.Inconclusive("Fix in 3.1");
            }

            [TestMethod]
            public void DoesNotInvokeEndEditingEventAfterBeginEditIsCalled()
            {
                var editableObject = new EditableObject();
                var editableObjectAsIEditableObject = (IEditableObject)editableObject;

                Assert.IsFalse(editableObject.EndEditingCalled);
                Assert.IsFalse(editableObject.OnEndEditCalled);

                editableObjectAsIEditableObject.EndEdit();

                Assert.IsFalse(editableObject.EndEditingCalled);
                Assert.IsFalse(editableObject.OnEndEditCalled);
            }

            [TestMethod]
            public void InvokesEndEditingEventAfterBeginEditIsCalled()
            {
                var editableObject = new EditableObject();
                var editableObjectAsIEditableObject = (IEditableObject)editableObject;

                Assert.IsFalse(editableObject.EndEditingCalled);
                Assert.IsFalse(editableObject.OnEndEditCalled);

                editableObjectAsIEditableObject.BeginEdit();
                editableObjectAsIEditableObject.EndEdit();

                Assert.IsTrue(editableObject.EndEditingCalled);
                Assert.IsTrue(editableObject.OnEndEditCalled);
            }
        }

        [TestClass]
        public class TheClearIsDirtyMethod
        {
            [TestMethod]
            public void CorrectlyRaisesPropertyChangedForIsDirty()
            {
                int isDirtyChangedCalls = 0;
                var model = new ClearIsDirtyModel();

                ((INotifyPropertyChanged)model).PropertyChanged += (sender, e) =>
                {
                    if (string.Equals(e.PropertyName, "IsDirty"))
                    {
                        isDirtyChangedCalls++;
                    }
                };

                ((IEditableObject)model).BeginEdit();

                // IsDirty change 1
                model.Name = "Me";

                // IsDirty change 2
                model.ClearIsDirty();

                // IsDirty change 3 + 4 (Name change back to null, and restoreof IsDirty)
                ((IEditableObject)model).CancelEdit();

                Assert.AreEqual(4, isDirtyChangedCalls);
            }
        }
    }
}