// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Linq;
using Xunit;

namespace System.Reflection.Emit.Tests
{
    public class EnumBuilderSetCustomAttribute2
    {
        private AssemblyBuilder _myAssemblyBuilder;
        private ModuleBuilder _myModuleBuilder;
        private EnumBuilder _myEnumBuilder;
        private ConstructorInfo _myInfo;

        private void CreateCallee()
        {
            AssemblyName myAssemblyName = new AssemblyName();
            myAssemblyName.Name = "EnumAssembly";
            _myAssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(myAssemblyName, AssemblyBuilderAccess.Run);
            _myModuleBuilder = TestLibrary.Utilities.GetModuleBuilder(_myAssemblyBuilder, "EnumModule.mod");
            _myInfo = typeof(EBMyAttribute2).GetConstructor(new Type[] { typeof(bool) });
        }

        [Fact]
        public void TestSetCustomAttribute()
        {
            CreateCallee();
            _myEnumBuilder = _myModuleBuilder.DefineEnum("myEnum", TypeAttributes.Public, typeof(int));
            _myEnumBuilder.CreateTypeInfo().AsType();
            CustomAttributeBuilder myCustomAttribute = new CustomAttributeBuilder(_myInfo, new object[] { true });
            _myEnumBuilder.SetCustomAttribute(myCustomAttribute);
            object[] objVals = _myEnumBuilder.GetCustomAttributes(true).Select(a => (object)a).ToArray();
            Assert.Equal(1, objVals.Length);
            Assert.True(objVals[0].Equals(new EBMyAttribute2(true)));
        }

        public class EBMyAttribute2 : Attribute
        {
            private bool _myBoolValue;

            public EBMyAttribute2(bool myBool)
            {
                _myBoolValue = myBool;
            }
        }
    }
}
