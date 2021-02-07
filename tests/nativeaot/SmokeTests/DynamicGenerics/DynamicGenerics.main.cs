// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System;
using System.Runtime;

public class EntryPointMain
{
    [RuntimeExport("Main")]
    public static void Main()
    {
        //CoreFXTestLibrary.Internal.TestInfo[] tests = new CoreFXTestLibrary.Internal.TestInfo[]{

        //activation.cs
        //TODO Add Type.MakeGenericType, Activator, Type.GetTypeInfo, GC
        //My.TestActivatorCreateInstance();
        //TODO Fix Assert
        //My.TestDefaultCtorInLazyGenerics();

        //expression.cs
        //TODO Add Type.MakeGenericType and Activator
        //Expressions.ExpressionsTesting.TestLdTokenResults();
        //Expressions.ExpressionsTesting.TestLdTokenResultsWithStructTypes();

        //genericmethods.cs
        //TODO Add Type.MakeGenericType, MethodInfo, Type.GetTypeInfo, Activator and List<T>
        //MakeGenMethod.Test.TestInstanceMethod();
        //MakeGenMethod.Test.TestStaticMethod();
        //TODO Add TypeInfo, Type.GetTypeInfo and MethodInfo
        //MakeGenMethod.Test.TestGenericMethodsWithEnumParametersHavingDefaultValues();
        //TODO Add MethodInfo, Type.GetTypeInfo
        //MakeGenMethod.Test.TestNoDictionaries();
        //TODO Add Type.MakeGenericType and Activator
        //MakeGenMethod.Test.TestGenMethodOnGenType();
        //TODO Add MethodInfo, Type.GetTypeInfo, Type.MakeGenericType, Type.Name and IEnumerable and Fix Delegate
        //MakeGenMethod.Test.TestReverseLookups();
        //TODO Add Func<U, V>, Type.MakeArrayType, MethodInfo and Type.GetTypeInfo
        //MakeGenMethod.Test.TestReverseLookupsWithArrayArg();

        //arrays.cs
        //TODO Add TypeInfo, Type.MakeArrayType, Type.MakeGenericType, Array.GetType?, Console.WriteLine(string, params), Activator, Type.ToString, Array.SetValue, Array.GetValue and IEnumerable
        //ArrayTests.ArrayTests.TestArrays();
        //ArrayTests.ArrayTests.TestDynamicArrays();
        //ArrayTests.ArrayTests.TestMDArrays();
        //ArrayTests.ArrayTests.TestArrayIndexOfNullableStructOfCanon_USG();
        //TODO Add Nullable<T>, Type.MakeGenericType, Array.CreateInstance and Activator
        //ArrayTests.ArrayTests.TestArrayIndexOfNullableStructOfCanon_Canon();

        //blockedtypes.cs
        //TODO Add Type.MakeGenericType and Activator
        //BlockedTypesTests.TestBlockedTypes();

        //constraints.cs
        //TODO Add Exception support and Type.MakeGenericType
        //ConstraintsTests.TestInvalidInstantiations();
        //ConstraintsTests.TestSpecialConstraints();
        //ConstraintsTests.TestTypeConstraints();

        //methodconstraints.cs
        //TODO Add Exception support
        //MethodConstraintsTests.TestInvalidInstantiations();
        //TODO Add Exception support, MethodInfo and Type.GetTypeInfo
        //MethodConstraintsTests.TestSpecialConstraints();
        //TODO Add Exception support, MethodInfo, Type.GetTypeInfo and Type.MakeGenericType
        //MethodConstraintsTests.TestTypeConstraints();
        //TODO Add Exception support
        //MethodConstraintsTests.TestMDTypeConstraints();

        //dictionary.cs
        //TODO Add Type.MakeGenericType, Activator, MethodInfo, Type.GetTypeInfo, Object.ToString, Exception Support and Object.GetType
        //Dictionaries.DictionariesTest.TestBasicDictionaryEntryTypes();
        //TODO Add Type.MakeGenericType
        //Dictionaries.DictionariesTest.StaticMethodFolding_Test();
        //TODO Add Type.MakeGenericType, Activator, Object.GetType, Exception Support and Delegate
        //Dictionaries.DictionariesTest.NullableTesting();
        //TODO Add Type.MakeGenericType, Type.GetTypeInfo, Activator and Type.ToString
        //TypeDictTestTypes.DictionariesTest.TestGenericTypeDictionary();
        //TODO Add StringBuilder, Type.MakeGenericType, Activator, Type.Name and IDisposable
        //MethodDictionaryTest.DictionariesTest.TestMethodDictionaries();
        //TODO Add Type.MakeGenericType and Activator
        //BaseTypeDict.Test.TestVirtCallTwoGenParams();
        //BaseTypeDict.Test.TestUsingPrimitiveTypes();
        //TODO Add IEnumerable, Type.MakeGenericType, Activator and Type.Name
        //BaseTypeDict.Test.TestBaseTypeDictionaries();
        //TODO Add Type.MakeGenericType and Activator
        //DictDependency.Test.TestIndirectDictionaryDependencies();
        //TODO Add Type.MakeGenericType, Activator, and Exception Support
        //CtorDict.DictionaryTesting.TestAllocationDictionaryEntryTypes();
        //TODO Add Type.MakeGenericType and Activator
        //MethodAndUnboxingStubTesting.Test.TestNoConstraints();
        //TODO Add Type.MakeGenericType, Activator, Object.GetType, Func<T>
        //ExistingInstantiations.Test.TestWithExistingInst();
        //TODO Add GC, Activator, Type.GetTypeInfo and Delegate
        //ExistingInstantiations.Test.TestInstantiationsWithExistingArrayTypeArgs();
        //TODO Add Type.MakeGenericType, Activator and MethodInfo
        //TemplateDependencyFromGenArgs.TestRunner.TemplateDependencyFromGenArgsTest();
#if UNIVERSAL_GENERICS
        *FieldLayoutTests.TestFieldLayoutMatchesBetweenStaticAndDynamic_Long();
        FieldLayoutTests.TestFieldLayoutMatchesBetweenStaticAndDynamic_Int64Enum();
        FieldLayoutTests.TestBoxingUSGCreatedNullable();*/
#endif
        //fieldreflection.cs
        //TODO Add Type.MakeGenericType, TypeInfo, Type.GetTypeInfo, Activator, FieldInfo and Func<U, V>
        //FieldReflectionTests.TestInstanceFieldsOnDerivedType();
        //FieldReflectionTests.TestInstanceFields();
        //TODO Add Type.MakeGenericType, TypeInfo, Type.GetTypeInfo, Func<U, V> and FieldInfo
        //FieldReflectionTests.TestStaticFields();
        //TODO Add Type.MakeGenericType, TypeInfo, Type.GetTypeInfo and FieldInfo
        //FieldReflectionTests.TestInitializedStaticFields();
        //FieldReflectionTests.TestFieldSetValueOnInstantiationsThatAlreadyExistButAreNotKnownToReflection();

        //interfaces.cs
        //TODO Add MethodInfo and Type.GetTypeInfo
        //InterfacesTests.TestGenericCollapsingInInterfaceMap();
        //TODO Add TypeInfo, Type.MakeGenericType and Type.GetTypeInfo
        //InterfacesTests.TestImplementedInterfaces();
        //TODO Add Type.MakeGenericType and Type.GetTypeInfo
        //InterfacesTests.TestBaseType();
        //TODO AdD Type.MakeGenericType, TypeInfo and Activator
        //InterfacesTests.TestInterfaceInvoke();
        //TODO Add Type.MakeGenericType and Activator
        //InterfacesTests.TestConstrainedCall();
#if !MULTIMODULE_BUILD
        //list.cs
        //TODO Add Object.GetType
        //DynamicListTests.TestGetRange();
        //TODO Add IEnumerable<T> and IEnumerator<T>
        //DynamicListTests.TestAddRange();
        //TODO Add Type.GetTypeInfo, Activator and Object.ReferenceEquals
        //DynamicListTests.TestAddRemove();
        //TODO Add IList<T> and Object.GetType 
        //DynamicListTests.TestIListOfT();
        //TODO Add ICollection<T> and Object.GetType
        //DynamicListTests.TestICollectionOfT();
        //TODO Add IList and Object.GetType
        //DynamicListTests.TestIList();
        //TODO Add ICollection
        //DynamicListTests.TestICollection();
        //TODO Add IReadOnlyList<T>
        //DynamicListTests.TestIReadOnlyListOfT();
        //TODO Add IReadOnlyCollection<T>
        //DynamicListTests.TestIReadOnlyCollectionOfT();
        //TODO Add MethodInfo, Type.GetTypeInfo, IList<T>, Type.MakeArrayType and Object.GetType
        //DynamicListTests.TestToArray();
        //TODO Add IEnumerable<T>
        //DynamicListTests.TestContains();
        //TODO Add Comparision<T>, IEnumerable<T>, Activator, MethodInfo, ParameterInfo
        //DynamicListTests.TestSortWithComparison();
        //TODO Add IEnumerable<T>, IComparer, Activator, MethodInfo and ParameterInfo
        //DynamicListTests.TestSortWithComparer();
#endif
        //RdExperienceTests.TestRdExperience();
        //statics.cs
        //TODO Add Type.MakeGenericType, Activator, String.Replace and GC
        //StaticsTests.TestStatics();
        //ThreadLocalStatics.TLSTesting.ThreadLocalStatics_Test();
#if UNIVERSAL_GENERICS
            global::UnivConstCalls.Test.TestRefTypeCallsOnNonGenClass();
            global::UnivConstCalls.Test.TestUSCCallsOnNonGenStruct();
            global::UnivConstCalls.Test.TestUSCCallsOnSharedGenStruct();
            global::UnivConstCalls.Test.TestUSCCallsOnUSCGenStruct();
            global::UnivConstCalls.Test.TestUSCNonGenInterfaceCallsOnStructs();
            global::UniversalGen.Test.TestInterlockedPrimitives();
            global::UniversalGen.Test.TestArraysAndGC();
            global::UniversalGen.Test.TestUSGByRefFunctionCalls();
            global::UniversalGen.Test.TestUSGSamples();
            global::UniversalGen.Test.TestMakeGenericType();
            global::UniversalGen.Test.TestUSCInstanceFieldUsage();
            global::UniversalGen.Test.TestUSCStaticFieldUsage();
            global::UniversalGen.Test.TestUSCThreadStaticFieldUsage();
            global::UniversalGen.Test.TestUSCStaticFieldLayoutCompat();
            global::UniversalGen.Test.TestUSCClassConstructorImplicit();
            global::UniversalGen.Test.TestUSCClassConstructorExplicit();
            global::UniversalGen.Test.TestUniversalGenericsGvmCall();
            global::PartialUSC.Test.TestVirtualCallsPartialUSGVTableMismatch();
            global::PartialUSC.Test.TestVirtualCalls();
            global::VirtualCalls.Test.TestVirtualCalls();
            global::CallingConvention.Test.TestInstancesOfKnownAndUnknownSizes();
            global::CallingConvention.Test.TestCallInstanceFunction();
            global::CallingConvention.Test.TestCallInterface();
            global::CallingConvention.Test.CallingConventionTest();
            global::DynamicInvoke.Test.TestDynamicInvoke();
            global::TypeLayout.Test.TestTypeGCDescs();
            global::TypeLayout.Test.StructsOfPrimitives();
            global::ActivatorCreateInstance.Test.TestCreateInstance();
            global::MultiThreadUSCCall.Test.CallsWithGCCollects();
            global::Heuristics.TestHeuristics.TestReflectionHeuristics();
            global::ArrayVarianceTest.Test.RunTest();
            global::IsInstTest.TestRunner.RunIsInstAndCheckCastTest();
            global::DelegateCallTest.TestRunner.TestCallMethodThroughUsgDelegate();
            global::FieldLayoutBugRepro.Runner.EntryPoint();
            global::DelegateTest.TestRunner.TestMethodCellsWithUSGTargetsUsedOnNonUSGInstantiations();
            global::ArrayExceptionsTest.Runner.ArrayExceptionsTest_String_Object();
            global::ArrayExceptionsTest.Runner.ArrayExceptionsTest_Int32_Int32();
            global::ArrayExceptionsTest.Runner.ArrayExceptionsTest_Int32_IntBasedEnum();
            global::ArrayExceptionsTest.Runner.ArrayExceptionsTest_UInt32_Int32();
            global::UnboxAnyTests.Runner.TestUnboxAnyToString();
            global::UnboxAnyTests.Runner.TestUnboxAnyToInt();
            global::UnboxAnyTests.Runner.TestUnboxAnyToIntBasedEnum();
            global::UnboxAnyTests.Runner.TestUnboxAnyToNullableInt();
            global::UnboxAnyTests.Runner.TestUnboxAnyToNullableIntBasedEnum();
            global::UnboxAnyTests.Runner.TestUnboxAnyToShort_NonUSG();
            global::UnboxAnyTests.Runner.TestUnboxAnyToShortBasedEnum_NonUSG();
            global::UnboxAnyTests.Runner.TestUnboxAnyToNullableShort_NonUSG();
            global::UnboxAnyTests.Runner.TestUnboxAnyToNullableShortBasedEnum_NonUSG();
            global::HFATest.Runner.HFATestEntryPoint();
            global::ComparerOfTTests.Runner.TestStructThatImplementsIComparable();
            global::ComparerOfTTests.Runner.TestStructThatImplementsIComparableOfObject();
            global::ComparerOfTTests.Runner.TestBoringStruct();
            global::DefaultValueDelegateParameterTests.Runner.TestCallUniversalGenericDelegate();
            global::ArrayOfGenericStructGCTests.Runner.TestArrayOfGenericStructGCTests();
            global::ArrayOfGenericStructGCTests.Runner.TestNonPointerSizedFinalField();
            global::DelegatesToStructMethods.Runner.TestDelegateInvokeToMethods();
            global::PartialUniversalGen.Test.TestOverrideMethodOnDerivedTypeWhereInstantiationArgsAreDifferentThanBaseType();
            global::PartialUniversalGen.Test.TestUniversalGenericThatDerivesFromBaseInstantiatedOverArray();
            global::PartialUniversalGen.Test.TestUniversalGenericThatUsesCanonicalGeneric();
            global::PartialUniversalGen.Test.TestUniversalGenericThatImplementsInterfaceOverArrayType();
            global::PartialUniversalGen.Test.TestUniversalGenericThatUsesCanonicalGenericMethod();
            global::PartialUniversalGen.Test.TestUniversalGenericThatUsesCanonicalGenericMethodWithActivatorCreateInstance();
            global::PartialUniversalGen.Test.TestUniversalGenericThatUsesCanonicalGenericType();
            global::PartialUniversalGen.Test.TestUniversalGenericThatUsesCanonicalGenericMethodWithConstraints();
            global::PartialUniversalGen.Test.TestDependenciesOfPartialUniversalCanonicalCode();
            global::PartialUniversalGen.Test.TestCornerCaseSealedVTableSlot();
#endif
        //B282745.cs
        //TODO Add Array.CreateInstance, GC and Object.GetType
        //B282745.testIntMDArrayWithPointerLikeValues();
        //B282745.testLongMDArrayWithPointerLikeValues();
        //B282745.testMDArrayWithPointerLikeValuesOfKnownStructType();
        //TODO Add Type.MakeGenericType, MethodInfo and Type.GetTypeInfo
        //B282745.testMDArrayWithPointerLikeValuesOfUnknownStructReferenceType();
        //B282745.testMDArrayWithPointerLikeValuesOfUnknownStructPrimitiveType();
        //TODO Add array.CreateInstance, IEnumerable, GC and Object.ReferenceEquals
        //B282745.testMDArrayWith3Dimensions();
#if UNIVERSAL_GENERICS
            global::B279085.TestB279085Repro();
#endif
        //GenericVirtualMethods.cs
        //TODO Add Func<U, V>
        //GenericVirtualMethods.TestCalls();
        //GenericVirtualMethods.TestLdFtnToGetStaticMethodOnGenericType();
        //TODO Add Func<U, V> and RuntimeReflectionExtensions
        //GenericVirtualMethods.TestLdFtnToInstanceGenericMethod();
        // https://github.com/dotnet/corert/issues/3460
        //GenericVirtualMethods.TestGenericExceptionType();
        //TODO Add TypeLoaderExports.cs
        //GenericVirtualMethods.TestCoAndContraVariantCalls();
        //};

        //bool passed = CoreFXTestLibrary.Internal.Runner.RunTests(tests, args);
        //CoreFXTestLibrary.Logger.LogInformation("Passed: {0}, Failed: {1}, Number of Tests Run: {2}", CoreFXTestLibrary.Internal.Runner.NumPassedTests, CoreFXTestLibrary.Internal.Runner.NumFailedTests, CoreFXTestLibrary.Internal.Runner.NumTests);
        //if (passed && CoreFXTestLibrary.Internal.Runner.NumPassedTests > 0)
        {
            // CoreFXTestLibrary.Logger.LogInformation("All tests PASSED.");
            //return 100;
        }
        //else
        {
            // CoreFXTestLibrary.Logger.LogInformation("{0} tests FAILED!", CoreFXTestLibrary.Internal.Runner.NumFailedTests);
            //return CoreFXTestLibrary.Internal.Runner.NumFailedTests == 100 ? 101 : CoreFXTestLibrary.Internal.Runner.NumFailedTests;
        }

        Console.WriteLine("Done");
        Console.ReadLine();
    }
}
