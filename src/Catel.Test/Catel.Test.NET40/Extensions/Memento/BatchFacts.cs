﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BatchFacts.cs" company="Catel development team">
//   Copyright (c) 2008 - 2014 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Catel.Test.Memento
{
    using Catel.Memento;

#if NETFX_CORE
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

    public class BatchFacts
    {
        [TestClass]
        public class TheActionCountProperty
        {
            [TestMethod]
            public void ReturnsZeroForEmptyBatch()
            {
                var batch = new Batch();

                Assert.AreEqual(0, batch.ActionCount);
            }

            [TestMethod]
            public void ReturnsOneForBatchWithOneAction()
            {
                var model = new Mocks.MockModel();

                var batch = new Batch();
                batch.AddAction(new PropertyChangeUndo(model, "Value", model.Value));

                Assert.AreEqual(1, batch.ActionCount);
            }
        }

        [TestClass]
        public class TheIsEmptyBatchProperty
        {
            [TestMethod]
            public void ReturnsTrueForEmptyBatch()
            {
                var batch = new Batch();
                
                Assert.IsTrue(batch.IsEmptyBatch);
            }

            [TestMethod]
            public void ReturnsFalseForBatchWithOneAction()
            {
                var model = new Mocks.MockModel();

                var batch = new Batch();
                batch.AddAction(new PropertyChangeUndo(model, "Value", model.Value));

                Assert.IsFalse(batch.IsEmptyBatch);
            }

            [TestMethod]
            public void ReturnsFalseForBatchWithMultipleActions()
            {
                var model1 = new Mocks.MockModel();
                var model2 = new Mocks.MockModel();

                var batch = new Batch();
                batch.AddAction(new PropertyChangeUndo(model1, "Value", model1.Value));
                batch.AddAction(new PropertyChangeUndo(model2, "Value", model2.Value));

                Assert.IsFalse(batch.IsEmptyBatch);
            }
        }

        [TestClass]
        public class TheIsSingleActionBatchProperty
        {
            [TestMethod]
            public void ReturnsFalseForEmptyBatch()
            {
                var batch = new Batch();

                Assert.IsFalse(batch.IsSingleActionBatch);
            }

            [TestMethod]
            public void ReturnsTrueForBatchWithOneAction()
            {
                var model = new Mocks.MockModel();

                var batch = new Batch();
                batch.AddAction(new PropertyChangeUndo(model, "Value", model.Value));

                Assert.IsTrue(batch.IsSingleActionBatch);
            }

            [TestMethod]
            public void ReturnsFalseForBatchWithMultipleActions()
            {
                var model1 = new Mocks.MockModel();
                var model2 = new Mocks.MockModel();

                var batch = new Batch();
                batch.AddAction(new PropertyChangeUndo(model1, "Value", model1.Value));
                batch.AddAction(new PropertyChangeUndo(model2, "Value", model2.Value));

                Assert.IsFalse(batch.IsSingleActionBatch);
            }
        }

        [TestClass]
        public class TheCanRedoProperty
        {
            [TestMethod]
            public void IsFalseWhenNoActionsCanRedo()
            {
                var model1 = new Mocks.MockModel();

                var batch = new Batch();
                batch.AddAction(new ActionUndo(model1, () => model1.Value = "Value"));

                Assert.IsFalse(batch.CanRedo);
            }

            [TestMethod]
            public void IsTrueWhenAtLeastOneActionCanRedo()
            {
                var model1 = new Mocks.MockModel();
                var model2 = new Mocks.MockModel();

                var batch = new Batch();
                batch.AddAction(new PropertyChangeUndo(model1, "Value", model1.Value));
                batch.AddAction(new PropertyChangeUndo(model2, "Value", model2.Value));

                Assert.IsTrue(batch.CanRedo);
            }
        }
    }
}