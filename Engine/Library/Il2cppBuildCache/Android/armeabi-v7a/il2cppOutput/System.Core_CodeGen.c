#include "pch-c.h"
#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include "codegen/il2cpp-codegen-metadata.h"





// 0x00000001 System.Exception System.Linq.Error::ArgumentNull(System.String)
extern void Error_ArgumentNull_m0EDA0D46D72CA692518E3E2EB75B48044D8FD41E (void);
// 0x00000002 System.Exception System.Linq.Error::MoreThanOneMatch()
extern void Error_MoreThanOneMatch_m4C4756AF34A76EF12F3B2B6D8C78DE547F0FBCF8 (void);
// 0x00000003 System.Exception System.Linq.Error::NoElements()
extern void Error_NoElements_mB89E91246572F009281D79730950808F17C3F353 (void);
// 0x00000004 System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable::Where(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x00000005 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable::Select(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TResult>)
// 0x00000006 System.Func`2<TSource,System.Boolean> System.Linq.Enumerable::CombinePredicates(System.Func`2<TSource,System.Boolean>,System.Func`2<TSource,System.Boolean>)
// 0x00000007 System.Func`2<TSource,TResult> System.Linq.Enumerable::CombineSelectors(System.Func`2<TSource,TMiddle>,System.Func`2<TMiddle,TResult>)
// 0x00000008 System.Linq.IOrderedEnumerable`1<TSource> System.Linq.Enumerable::OrderBy(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TKey>)
// 0x00000009 System.Linq.IOrderedEnumerable`1<TSource> System.Linq.Enumerable::ThenBy(System.Linq.IOrderedEnumerable`1<TSource>,System.Func`2<TSource,TKey>)
// 0x0000000A System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable::Distinct(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x0000000B System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable::DistinctIterator(System.Collections.Generic.IEnumerable`1<TSource>,System.Collections.Generic.IEqualityComparer`1<TSource>)
// 0x0000000C TSource[] System.Linq.Enumerable::ToArray(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x0000000D System.Collections.Generic.List`1<TSource> System.Linq.Enumerable::ToList(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x0000000E TSource System.Linq.Enumerable::First(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x0000000F TSource System.Linq.Enumerable::Last(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x00000010 TSource System.Linq.Enumerable::SingleOrDefault(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x00000011 System.Boolean System.Linq.Enumerable::Any(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x00000012 System.Boolean System.Linq.Enumerable::Any(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x00000013 System.Int32 System.Linq.Enumerable::Count(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x00000014 System.Boolean System.Linq.Enumerable::Contains(System.Collections.Generic.IEnumerable`1<TSource>,TSource)
// 0x00000015 System.Boolean System.Linq.Enumerable::Contains(System.Collections.Generic.IEnumerable`1<TSource>,TSource,System.Collections.Generic.IEqualityComparer`1<TSource>)
// 0x00000016 System.Void System.Linq.Enumerable/Iterator`1::.ctor()
// 0x00000017 TSource System.Linq.Enumerable/Iterator`1::get_Current()
// 0x00000018 System.Linq.Enumerable/Iterator`1<TSource> System.Linq.Enumerable/Iterator`1::Clone()
// 0x00000019 System.Void System.Linq.Enumerable/Iterator`1::Dispose()
// 0x0000001A System.Collections.Generic.IEnumerator`1<TSource> System.Linq.Enumerable/Iterator`1::GetEnumerator()
// 0x0000001B System.Boolean System.Linq.Enumerable/Iterator`1::MoveNext()
// 0x0000001C System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable/Iterator`1::Select(System.Func`2<TSource,TResult>)
// 0x0000001D System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable/Iterator`1::Where(System.Func`2<TSource,System.Boolean>)
// 0x0000001E System.Object System.Linq.Enumerable/Iterator`1::System.Collections.IEnumerator.get_Current()
// 0x0000001F System.Collections.IEnumerator System.Linq.Enumerable/Iterator`1::System.Collections.IEnumerable.GetEnumerator()
// 0x00000020 System.Void System.Linq.Enumerable/Iterator`1::System.Collections.IEnumerator.Reset()
// 0x00000021 System.Void System.Linq.Enumerable/WhereEnumerableIterator`1::.ctor(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x00000022 System.Linq.Enumerable/Iterator`1<TSource> System.Linq.Enumerable/WhereEnumerableIterator`1::Clone()
// 0x00000023 System.Void System.Linq.Enumerable/WhereEnumerableIterator`1::Dispose()
// 0x00000024 System.Boolean System.Linq.Enumerable/WhereEnumerableIterator`1::MoveNext()
// 0x00000025 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable/WhereEnumerableIterator`1::Select(System.Func`2<TSource,TResult>)
// 0x00000026 System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable/WhereEnumerableIterator`1::Where(System.Func`2<TSource,System.Boolean>)
// 0x00000027 System.Void System.Linq.Enumerable/WhereArrayIterator`1::.ctor(TSource[],System.Func`2<TSource,System.Boolean>)
// 0x00000028 System.Linq.Enumerable/Iterator`1<TSource> System.Linq.Enumerable/WhereArrayIterator`1::Clone()
// 0x00000029 System.Boolean System.Linq.Enumerable/WhereArrayIterator`1::MoveNext()
// 0x0000002A System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable/WhereArrayIterator`1::Select(System.Func`2<TSource,TResult>)
// 0x0000002B System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable/WhereArrayIterator`1::Where(System.Func`2<TSource,System.Boolean>)
// 0x0000002C System.Void System.Linq.Enumerable/WhereListIterator`1::.ctor(System.Collections.Generic.List`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x0000002D System.Linq.Enumerable/Iterator`1<TSource> System.Linq.Enumerable/WhereListIterator`1::Clone()
// 0x0000002E System.Boolean System.Linq.Enumerable/WhereListIterator`1::MoveNext()
// 0x0000002F System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable/WhereListIterator`1::Select(System.Func`2<TSource,TResult>)
// 0x00000030 System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable/WhereListIterator`1::Where(System.Func`2<TSource,System.Boolean>)
// 0x00000031 System.Void System.Linq.Enumerable/WhereSelectEnumerableIterator`2::.ctor(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>,System.Func`2<TSource,TResult>)
// 0x00000032 System.Linq.Enumerable/Iterator`1<TResult> System.Linq.Enumerable/WhereSelectEnumerableIterator`2::Clone()
// 0x00000033 System.Void System.Linq.Enumerable/WhereSelectEnumerableIterator`2::Dispose()
// 0x00000034 System.Boolean System.Linq.Enumerable/WhereSelectEnumerableIterator`2::MoveNext()
// 0x00000035 System.Collections.Generic.IEnumerable`1<TResult2> System.Linq.Enumerable/WhereSelectEnumerableIterator`2::Select(System.Func`2<TResult,TResult2>)
// 0x00000036 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable/WhereSelectEnumerableIterator`2::Where(System.Func`2<TResult,System.Boolean>)
// 0x00000037 System.Void System.Linq.Enumerable/WhereSelectArrayIterator`2::.ctor(TSource[],System.Func`2<TSource,System.Boolean>,System.Func`2<TSource,TResult>)
// 0x00000038 System.Linq.Enumerable/Iterator`1<TResult> System.Linq.Enumerable/WhereSelectArrayIterator`2::Clone()
// 0x00000039 System.Boolean System.Linq.Enumerable/WhereSelectArrayIterator`2::MoveNext()
// 0x0000003A System.Collections.Generic.IEnumerable`1<TResult2> System.Linq.Enumerable/WhereSelectArrayIterator`2::Select(System.Func`2<TResult,TResult2>)
// 0x0000003B System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable/WhereSelectArrayIterator`2::Where(System.Func`2<TResult,System.Boolean>)
// 0x0000003C System.Void System.Linq.Enumerable/WhereSelectListIterator`2::.ctor(System.Collections.Generic.List`1<TSource>,System.Func`2<TSource,System.Boolean>,System.Func`2<TSource,TResult>)
// 0x0000003D System.Linq.Enumerable/Iterator`1<TResult> System.Linq.Enumerable/WhereSelectListIterator`2::Clone()
// 0x0000003E System.Boolean System.Linq.Enumerable/WhereSelectListIterator`2::MoveNext()
// 0x0000003F System.Collections.Generic.IEnumerable`1<TResult2> System.Linq.Enumerable/WhereSelectListIterator`2::Select(System.Func`2<TResult,TResult2>)
// 0x00000040 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable/WhereSelectListIterator`2::Where(System.Func`2<TResult,System.Boolean>)
// 0x00000041 System.Void System.Linq.Enumerable/<>c__DisplayClass6_0`1::.ctor()
// 0x00000042 System.Boolean System.Linq.Enumerable/<>c__DisplayClass6_0`1::<CombinePredicates>b__0(TSource)
// 0x00000043 System.Void System.Linq.Enumerable/<>c__DisplayClass7_0`3::.ctor()
// 0x00000044 TResult System.Linq.Enumerable/<>c__DisplayClass7_0`3::<CombineSelectors>b__0(TSource)
// 0x00000045 System.Void System.Linq.Enumerable/<DistinctIterator>d__68`1::.ctor(System.Int32)
// 0x00000046 System.Void System.Linq.Enumerable/<DistinctIterator>d__68`1::System.IDisposable.Dispose()
// 0x00000047 System.Boolean System.Linq.Enumerable/<DistinctIterator>d__68`1::MoveNext()
// 0x00000048 System.Void System.Linq.Enumerable/<DistinctIterator>d__68`1::<>m__Finally1()
// 0x00000049 TSource System.Linq.Enumerable/<DistinctIterator>d__68`1::System.Collections.Generic.IEnumerator<TSource>.get_Current()
// 0x0000004A System.Void System.Linq.Enumerable/<DistinctIterator>d__68`1::System.Collections.IEnumerator.Reset()
// 0x0000004B System.Object System.Linq.Enumerable/<DistinctIterator>d__68`1::System.Collections.IEnumerator.get_Current()
// 0x0000004C System.Collections.Generic.IEnumerator`1<TSource> System.Linq.Enumerable/<DistinctIterator>d__68`1::System.Collections.Generic.IEnumerable<TSource>.GetEnumerator()
// 0x0000004D System.Collections.IEnumerator System.Linq.Enumerable/<DistinctIterator>d__68`1::System.Collections.IEnumerable.GetEnumerator()
// 0x0000004E System.Linq.IOrderedEnumerable`1<TElement> System.Linq.IOrderedEnumerable`1::CreateOrderedEnumerable(System.Func`2<TElement,TKey>,System.Collections.Generic.IComparer`1<TKey>,System.Boolean)
// 0x0000004F System.Void System.Linq.Set`1::.ctor(System.Collections.Generic.IEqualityComparer`1<TElement>)
// 0x00000050 System.Boolean System.Linq.Set`1::Add(TElement)
// 0x00000051 System.Boolean System.Linq.Set`1::Find(TElement,System.Boolean)
// 0x00000052 System.Void System.Linq.Set`1::Resize()
// 0x00000053 System.Int32 System.Linq.Set`1::InternalGetHashCode(TElement)
// 0x00000054 System.Collections.Generic.IEnumerator`1<TElement> System.Linq.OrderedEnumerable`1::GetEnumerator()
// 0x00000055 System.Linq.EnumerableSorter`1<TElement> System.Linq.OrderedEnumerable`1::GetEnumerableSorter(System.Linq.EnumerableSorter`1<TElement>)
// 0x00000056 System.Collections.IEnumerator System.Linq.OrderedEnumerable`1::System.Collections.IEnumerable.GetEnumerator()
// 0x00000057 System.Linq.IOrderedEnumerable`1<TElement> System.Linq.OrderedEnumerable`1::System.Linq.IOrderedEnumerable<TElement>.CreateOrderedEnumerable(System.Func`2<TElement,TKey>,System.Collections.Generic.IComparer`1<TKey>,System.Boolean)
// 0x00000058 System.Void System.Linq.OrderedEnumerable`1::.ctor()
// 0x00000059 System.Void System.Linq.OrderedEnumerable`1/<GetEnumerator>d__1::.ctor(System.Int32)
// 0x0000005A System.Void System.Linq.OrderedEnumerable`1/<GetEnumerator>d__1::System.IDisposable.Dispose()
// 0x0000005B System.Boolean System.Linq.OrderedEnumerable`1/<GetEnumerator>d__1::MoveNext()
// 0x0000005C TElement System.Linq.OrderedEnumerable`1/<GetEnumerator>d__1::System.Collections.Generic.IEnumerator<TElement>.get_Current()
// 0x0000005D System.Void System.Linq.OrderedEnumerable`1/<GetEnumerator>d__1::System.Collections.IEnumerator.Reset()
// 0x0000005E System.Object System.Linq.OrderedEnumerable`1/<GetEnumerator>d__1::System.Collections.IEnumerator.get_Current()
// 0x0000005F System.Void System.Linq.OrderedEnumerable`2::.ctor(System.Collections.Generic.IEnumerable`1<TElement>,System.Func`2<TElement,TKey>,System.Collections.Generic.IComparer`1<TKey>,System.Boolean)
// 0x00000060 System.Linq.EnumerableSorter`1<TElement> System.Linq.OrderedEnumerable`2::GetEnumerableSorter(System.Linq.EnumerableSorter`1<TElement>)
// 0x00000061 System.Void System.Linq.EnumerableSorter`1::ComputeKeys(TElement[],System.Int32)
// 0x00000062 System.Int32 System.Linq.EnumerableSorter`1::CompareKeys(System.Int32,System.Int32)
// 0x00000063 System.Int32[] System.Linq.EnumerableSorter`1::Sort(TElement[],System.Int32)
// 0x00000064 System.Void System.Linq.EnumerableSorter`1::QuickSort(System.Int32[],System.Int32,System.Int32)
// 0x00000065 System.Void System.Linq.EnumerableSorter`1::.ctor()
// 0x00000066 System.Void System.Linq.EnumerableSorter`2::.ctor(System.Func`2<TElement,TKey>,System.Collections.Generic.IComparer`1<TKey>,System.Boolean,System.Linq.EnumerableSorter`1<TElement>)
// 0x00000067 System.Void System.Linq.EnumerableSorter`2::ComputeKeys(TElement[],System.Int32)
// 0x00000068 System.Int32 System.Linq.EnumerableSorter`2::CompareKeys(System.Int32,System.Int32)
// 0x00000069 System.Void System.Linq.Buffer`1::.ctor(System.Collections.Generic.IEnumerable`1<TElement>)
// 0x0000006A TElement[] System.Linq.Buffer`1::ToArray()
// 0x0000006B System.Void System.Collections.Generic.HashSet`1::.ctor()
// 0x0000006C System.Void System.Collections.Generic.HashSet`1::.ctor(System.Collections.Generic.IEqualityComparer`1<T>)
// 0x0000006D System.Void System.Collections.Generic.HashSet`1::.ctor(System.Collections.Generic.IEnumerable`1<T>)
// 0x0000006E System.Void System.Collections.Generic.HashSet`1::.ctor(System.Collections.Generic.IEnumerable`1<T>,System.Collections.Generic.IEqualityComparer`1<T>)
// 0x0000006F System.Void System.Collections.Generic.HashSet`1::.ctor(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)
// 0x00000070 System.Void System.Collections.Generic.HashSet`1::CopyFrom(System.Collections.Generic.HashSet`1<T>)
// 0x00000071 System.Void System.Collections.Generic.HashSet`1::System.Collections.Generic.ICollection<T>.Add(T)
// 0x00000072 System.Void System.Collections.Generic.HashSet`1::Clear()
// 0x00000073 System.Boolean System.Collections.Generic.HashSet`1::Contains(T)
// 0x00000074 System.Void System.Collections.Generic.HashSet`1::CopyTo(T[],System.Int32)
// 0x00000075 System.Boolean System.Collections.Generic.HashSet`1::Remove(T)
// 0x00000076 System.Int32 System.Collections.Generic.HashSet`1::get_Count()
// 0x00000077 System.Boolean System.Collections.Generic.HashSet`1::System.Collections.Generic.ICollection<T>.get_IsReadOnly()
// 0x00000078 System.Collections.Generic.HashSet`1/Enumerator<T> System.Collections.Generic.HashSet`1::GetEnumerator()
// 0x00000079 System.Collections.Generic.IEnumerator`1<T> System.Collections.Generic.HashSet`1::System.Collections.Generic.IEnumerable<T>.GetEnumerator()
// 0x0000007A System.Collections.IEnumerator System.Collections.Generic.HashSet`1::System.Collections.IEnumerable.GetEnumerator()
// 0x0000007B System.Void System.Collections.Generic.HashSet`1::GetObjectData(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)
// 0x0000007C System.Void System.Collections.Generic.HashSet`1::OnDeserialization(System.Object)
// 0x0000007D System.Boolean System.Collections.Generic.HashSet`1::Add(T)
// 0x0000007E System.Void System.Collections.Generic.HashSet`1::UnionWith(System.Collections.Generic.IEnumerable`1<T>)
// 0x0000007F System.Void System.Collections.Generic.HashSet`1::CopyTo(T[])
// 0x00000080 System.Void System.Collections.Generic.HashSet`1::CopyTo(T[],System.Int32,System.Int32)
// 0x00000081 System.Collections.Generic.IEqualityComparer`1<T> System.Collections.Generic.HashSet`1::get_Comparer()
// 0x00000082 System.Void System.Collections.Generic.HashSet`1::TrimExcess()
// 0x00000083 System.Void System.Collections.Generic.HashSet`1::Initialize(System.Int32)
// 0x00000084 System.Void System.Collections.Generic.HashSet`1::IncreaseCapacity()
// 0x00000085 System.Void System.Collections.Generic.HashSet`1::SetCapacity(System.Int32)
// 0x00000086 System.Boolean System.Collections.Generic.HashSet`1::AddIfNotPresent(T)
// 0x00000087 System.Void System.Collections.Generic.HashSet`1::AddValue(System.Int32,System.Int32,T)
// 0x00000088 System.Boolean System.Collections.Generic.HashSet`1::AreEqualityComparersEqual(System.Collections.Generic.HashSet`1<T>,System.Collections.Generic.HashSet`1<T>)
// 0x00000089 System.Int32 System.Collections.Generic.HashSet`1::InternalGetHashCode(T)
// 0x0000008A System.Void System.Collections.Generic.HashSet`1/Enumerator::.ctor(System.Collections.Generic.HashSet`1<T>)
// 0x0000008B System.Void System.Collections.Generic.HashSet`1/Enumerator::Dispose()
// 0x0000008C System.Boolean System.Collections.Generic.HashSet`1/Enumerator::MoveNext()
// 0x0000008D T System.Collections.Generic.HashSet`1/Enumerator::get_Current()
// 0x0000008E System.Object System.Collections.Generic.HashSet`1/Enumerator::System.Collections.IEnumerator.get_Current()
// 0x0000008F System.Void System.Collections.Generic.HashSet`1/Enumerator::System.Collections.IEnumerator.Reset()
static Il2CppMethodPointer s_methodPointers[143] = 
{
	Error_ArgumentNull_m0EDA0D46D72CA692518E3E2EB75B48044D8FD41E,
	Error_MoreThanOneMatch_m4C4756AF34A76EF12F3B2B6D8C78DE547F0FBCF8,
	Error_NoElements_mB89E91246572F009281D79730950808F17C3F353,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
};
static const int32_t s_InvokerIndices[143] = 
{
	2300,
	2383,
	2383,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
	-1,
};
static const Il2CppTokenRangePair s_rgctxIndices[44] = 
{
	{ 0x02000004, { 65, 4 } },
	{ 0x02000005, { 69, 9 } },
	{ 0x02000006, { 80, 7 } },
	{ 0x02000007, { 89, 10 } },
	{ 0x02000008, { 101, 11 } },
	{ 0x02000009, { 115, 9 } },
	{ 0x0200000A, { 127, 12 } },
	{ 0x0200000B, { 142, 1 } },
	{ 0x0200000C, { 143, 2 } },
	{ 0x0200000D, { 145, 11 } },
	{ 0x0200000F, { 156, 8 } },
	{ 0x02000011, { 164, 3 } },
	{ 0x02000012, { 169, 5 } },
	{ 0x02000013, { 174, 7 } },
	{ 0x02000014, { 181, 3 } },
	{ 0x02000015, { 184, 7 } },
	{ 0x02000016, { 191, 4 } },
	{ 0x02000017, { 195, 34 } },
	{ 0x02000019, { 229, 2 } },
	{ 0x06000004, { 0, 10 } },
	{ 0x06000005, { 10, 10 } },
	{ 0x06000006, { 20, 5 } },
	{ 0x06000007, { 25, 5 } },
	{ 0x06000008, { 30, 2 } },
	{ 0x06000009, { 32, 1 } },
	{ 0x0600000A, { 33, 1 } },
	{ 0x0600000B, { 34, 2 } },
	{ 0x0600000C, { 36, 3 } },
	{ 0x0600000D, { 39, 2 } },
	{ 0x0600000E, { 41, 4 } },
	{ 0x0600000F, { 45, 4 } },
	{ 0x06000010, { 49, 3 } },
	{ 0x06000011, { 52, 1 } },
	{ 0x06000012, { 53, 3 } },
	{ 0x06000013, { 56, 2 } },
	{ 0x06000014, { 58, 2 } },
	{ 0x06000015, { 60, 5 } },
	{ 0x06000025, { 78, 2 } },
	{ 0x0600002A, { 87, 2 } },
	{ 0x0600002F, { 99, 2 } },
	{ 0x06000035, { 112, 3 } },
	{ 0x0600003A, { 124, 3 } },
	{ 0x0600003F, { 139, 3 } },
	{ 0x06000057, { 167, 2 } },
};
static const Il2CppRGCTXDefinition s_rgctxValues[231] = 
{
	{ (Il2CppRGCTXDataType)2, 1270 },
	{ (Il2CppRGCTXDataType)3, 4137 },
	{ (Il2CppRGCTXDataType)2, 2103 },
	{ (Il2CppRGCTXDataType)2, 1785 },
	{ (Il2CppRGCTXDataType)3, 7100 },
	{ (Il2CppRGCTXDataType)2, 1348 },
	{ (Il2CppRGCTXDataType)2, 1792 },
	{ (Il2CppRGCTXDataType)3, 7118 },
	{ (Il2CppRGCTXDataType)2, 1787 },
	{ (Il2CppRGCTXDataType)3, 7107 },
	{ (Il2CppRGCTXDataType)2, 1271 },
	{ (Il2CppRGCTXDataType)3, 4138 },
	{ (Il2CppRGCTXDataType)2, 2124 },
	{ (Il2CppRGCTXDataType)2, 1794 },
	{ (Il2CppRGCTXDataType)3, 7125 },
	{ (Il2CppRGCTXDataType)2, 1368 },
	{ (Il2CppRGCTXDataType)2, 1802 },
	{ (Il2CppRGCTXDataType)3, 7144 },
	{ (Il2CppRGCTXDataType)2, 1798 },
	{ (Il2CppRGCTXDataType)3, 7134 },
	{ (Il2CppRGCTXDataType)2, 472 },
	{ (Il2CppRGCTXDataType)3, 24 },
	{ (Il2CppRGCTXDataType)3, 25 },
	{ (Il2CppRGCTXDataType)2, 824 },
	{ (Il2CppRGCTXDataType)3, 2994 },
	{ (Il2CppRGCTXDataType)2, 473 },
	{ (Il2CppRGCTXDataType)3, 28 },
	{ (Il2CppRGCTXDataType)3, 29 },
	{ (Il2CppRGCTXDataType)2, 830 },
	{ (Il2CppRGCTXDataType)3, 2996 },
	{ (Il2CppRGCTXDataType)2, 1593 },
	{ (Il2CppRGCTXDataType)3, 6140 },
	{ (Il2CppRGCTXDataType)3, 3350 },
	{ (Il2CppRGCTXDataType)3, 8674 },
	{ (Il2CppRGCTXDataType)2, 476 },
	{ (Il2CppRGCTXDataType)3, 39 },
	{ (Il2CppRGCTXDataType)2, 528 },
	{ (Il2CppRGCTXDataType)3, 453 },
	{ (Il2CppRGCTXDataType)3, 454 },
	{ (Il2CppRGCTXDataType)2, 1349 },
	{ (Il2CppRGCTXDataType)3, 4441 },
	{ (Il2CppRGCTXDataType)2, 1213 },
	{ (Il2CppRGCTXDataType)2, 928 },
	{ (Il2CppRGCTXDataType)2, 1010 },
	{ (Il2CppRGCTXDataType)2, 1074 },
	{ (Il2CppRGCTXDataType)2, 1214 },
	{ (Il2CppRGCTXDataType)2, 929 },
	{ (Il2CppRGCTXDataType)2, 1011 },
	{ (Il2CppRGCTXDataType)2, 1075 },
	{ (Il2CppRGCTXDataType)2, 1012 },
	{ (Il2CppRGCTXDataType)2, 1076 },
	{ (Il2CppRGCTXDataType)3, 2995 },
	{ (Il2CppRGCTXDataType)2, 1001 },
	{ (Il2CppRGCTXDataType)2, 1002 },
	{ (Il2CppRGCTXDataType)2, 1072 },
	{ (Il2CppRGCTXDataType)3, 2993 },
	{ (Il2CppRGCTXDataType)2, 927 },
	{ (Il2CppRGCTXDataType)2, 1009 },
	{ (Il2CppRGCTXDataType)2, 926 },
	{ (Il2CppRGCTXDataType)3, 8668 },
	{ (Il2CppRGCTXDataType)3, 2651 },
	{ (Il2CppRGCTXDataType)2, 736 },
	{ (Il2CppRGCTXDataType)2, 1004 },
	{ (Il2CppRGCTXDataType)2, 1073 },
	{ (Il2CppRGCTXDataType)2, 1125 },
	{ (Il2CppRGCTXDataType)3, 4139 },
	{ (Il2CppRGCTXDataType)3, 4141 },
	{ (Il2CppRGCTXDataType)2, 337 },
	{ (Il2CppRGCTXDataType)3, 4140 },
	{ (Il2CppRGCTXDataType)3, 4149 },
	{ (Il2CppRGCTXDataType)2, 1274 },
	{ (Il2CppRGCTXDataType)2, 1788 },
	{ (Il2CppRGCTXDataType)3, 7108 },
	{ (Il2CppRGCTXDataType)3, 4150 },
	{ (Il2CppRGCTXDataType)2, 1047 },
	{ (Il2CppRGCTXDataType)2, 1096 },
	{ (Il2CppRGCTXDataType)3, 3002 },
	{ (Il2CppRGCTXDataType)3, 8659 },
	{ (Il2CppRGCTXDataType)2, 1799 },
	{ (Il2CppRGCTXDataType)3, 7135 },
	{ (Il2CppRGCTXDataType)3, 4142 },
	{ (Il2CppRGCTXDataType)2, 1273 },
	{ (Il2CppRGCTXDataType)2, 1786 },
	{ (Il2CppRGCTXDataType)3, 7101 },
	{ (Il2CppRGCTXDataType)3, 3001 },
	{ (Il2CppRGCTXDataType)3, 4143 },
	{ (Il2CppRGCTXDataType)3, 8658 },
	{ (Il2CppRGCTXDataType)2, 1795 },
	{ (Il2CppRGCTXDataType)3, 7126 },
	{ (Il2CppRGCTXDataType)3, 4156 },
	{ (Il2CppRGCTXDataType)2, 1275 },
	{ (Il2CppRGCTXDataType)2, 1793 },
	{ (Il2CppRGCTXDataType)3, 7119 },
	{ (Il2CppRGCTXDataType)3, 4484 },
	{ (Il2CppRGCTXDataType)3, 2221 },
	{ (Il2CppRGCTXDataType)3, 3003 },
	{ (Il2CppRGCTXDataType)3, 2220 },
	{ (Il2CppRGCTXDataType)3, 4157 },
	{ (Il2CppRGCTXDataType)3, 8660 },
	{ (Il2CppRGCTXDataType)2, 1803 },
	{ (Il2CppRGCTXDataType)3, 7145 },
	{ (Il2CppRGCTXDataType)3, 4170 },
	{ (Il2CppRGCTXDataType)2, 1277 },
	{ (Il2CppRGCTXDataType)2, 1801 },
	{ (Il2CppRGCTXDataType)3, 7137 },
	{ (Il2CppRGCTXDataType)3, 4171 },
	{ (Il2CppRGCTXDataType)2, 1050 },
	{ (Il2CppRGCTXDataType)2, 1099 },
	{ (Il2CppRGCTXDataType)3, 3007 },
	{ (Il2CppRGCTXDataType)3, 3006 },
	{ (Il2CppRGCTXDataType)2, 1790 },
	{ (Il2CppRGCTXDataType)3, 7110 },
	{ (Il2CppRGCTXDataType)3, 8663 },
	{ (Il2CppRGCTXDataType)2, 1800 },
	{ (Il2CppRGCTXDataType)3, 7136 },
	{ (Il2CppRGCTXDataType)3, 4163 },
	{ (Il2CppRGCTXDataType)2, 1276 },
	{ (Il2CppRGCTXDataType)2, 1797 },
	{ (Il2CppRGCTXDataType)3, 7128 },
	{ (Il2CppRGCTXDataType)3, 3005 },
	{ (Il2CppRGCTXDataType)3, 3004 },
	{ (Il2CppRGCTXDataType)3, 4164 },
	{ (Il2CppRGCTXDataType)2, 1789 },
	{ (Il2CppRGCTXDataType)3, 7109 },
	{ (Il2CppRGCTXDataType)3, 8662 },
	{ (Il2CppRGCTXDataType)2, 1796 },
	{ (Il2CppRGCTXDataType)3, 7127 },
	{ (Il2CppRGCTXDataType)3, 4177 },
	{ (Il2CppRGCTXDataType)2, 1278 },
	{ (Il2CppRGCTXDataType)2, 1805 },
	{ (Il2CppRGCTXDataType)3, 7147 },
	{ (Il2CppRGCTXDataType)3, 4485 },
	{ (Il2CppRGCTXDataType)3, 2223 },
	{ (Il2CppRGCTXDataType)3, 3009 },
	{ (Il2CppRGCTXDataType)3, 3008 },
	{ (Il2CppRGCTXDataType)3, 2222 },
	{ (Il2CppRGCTXDataType)3, 4178 },
	{ (Il2CppRGCTXDataType)2, 1791 },
	{ (Il2CppRGCTXDataType)3, 7111 },
	{ (Il2CppRGCTXDataType)3, 8664 },
	{ (Il2CppRGCTXDataType)2, 1804 },
	{ (Il2CppRGCTXDataType)3, 7146 },
	{ (Il2CppRGCTXDataType)3, 2999 },
	{ (Il2CppRGCTXDataType)3, 3000 },
	{ (Il2CppRGCTXDataType)3, 3010 },
	{ (Il2CppRGCTXDataType)3, 41 },
	{ (Il2CppRGCTXDataType)2, 1640 },
	{ (Il2CppRGCTXDataType)3, 6369 },
	{ (Il2CppRGCTXDataType)2, 1042 },
	{ (Il2CppRGCTXDataType)2, 1092 },
	{ (Il2CppRGCTXDataType)3, 6370 },
	{ (Il2CppRGCTXDataType)3, 43 },
	{ (Il2CppRGCTXDataType)2, 335 },
	{ (Il2CppRGCTXDataType)2, 477 },
	{ (Il2CppRGCTXDataType)3, 40 },
	{ (Il2CppRGCTXDataType)3, 42 },
	{ (Il2CppRGCTXDataType)3, 2687 },
	{ (Il2CppRGCTXDataType)2, 751 },
	{ (Il2CppRGCTXDataType)2, 2183 },
	{ (Il2CppRGCTXDataType)3, 6366 },
	{ (Il2CppRGCTXDataType)3, 6367 },
	{ (Il2CppRGCTXDataType)2, 1139 },
	{ (Il2CppRGCTXDataType)3, 6368 },
	{ (Il2CppRGCTXDataType)2, 288 },
	{ (Il2CppRGCTXDataType)2, 478 },
	{ (Il2CppRGCTXDataType)3, 53 },
	{ (Il2CppRGCTXDataType)3, 6127 },
	{ (Il2CppRGCTXDataType)2, 1594 },
	{ (Il2CppRGCTXDataType)3, 6141 },
	{ (Il2CppRGCTXDataType)2, 529 },
	{ (Il2CppRGCTXDataType)3, 455 },
	{ (Il2CppRGCTXDataType)3, 6133 },
	{ (Il2CppRGCTXDataType)3, 2201 },
	{ (Il2CppRGCTXDataType)2, 356 },
	{ (Il2CppRGCTXDataType)3, 6128 },
	{ (Il2CppRGCTXDataType)2, 1590 },
	{ (Il2CppRGCTXDataType)3, 481 },
	{ (Il2CppRGCTXDataType)2, 541 },
	{ (Il2CppRGCTXDataType)2, 722 },
	{ (Il2CppRGCTXDataType)3, 2207 },
	{ (Il2CppRGCTXDataType)3, 6129 },
	{ (Il2CppRGCTXDataType)3, 2196 },
	{ (Il2CppRGCTXDataType)3, 2197 },
	{ (Il2CppRGCTXDataType)3, 2195 },
	{ (Il2CppRGCTXDataType)3, 2198 },
	{ (Il2CppRGCTXDataType)2, 718 },
	{ (Il2CppRGCTXDataType)2, 2167 },
	{ (Il2CppRGCTXDataType)3, 2998 },
	{ (Il2CppRGCTXDataType)3, 2200 },
	{ (Il2CppRGCTXDataType)2, 989 },
	{ (Il2CppRGCTXDataType)3, 2199 },
	{ (Il2CppRGCTXDataType)2, 930 },
	{ (Il2CppRGCTXDataType)2, 2127 },
	{ (Il2CppRGCTXDataType)2, 1023 },
	{ (Il2CppRGCTXDataType)2, 1077 },
	{ (Il2CppRGCTXDataType)3, 2670 },
	{ (Il2CppRGCTXDataType)2, 745 },
	{ (Il2CppRGCTXDataType)3, 3201 },
	{ (Il2CppRGCTXDataType)3, 3202 },
	{ (Il2CppRGCTXDataType)2, 911 },
	{ (Il2CppRGCTXDataType)3, 3205 },
	{ (Il2CppRGCTXDataType)2, 911 },
	{ (Il2CppRGCTXDataType)3, 3206 },
	{ (Il2CppRGCTXDataType)2, 931 },
	{ (Il2CppRGCTXDataType)3, 3210 },
	{ (Il2CppRGCTXDataType)3, 3214 },
	{ (Il2CppRGCTXDataType)3, 3213 },
	{ (Il2CppRGCTXDataType)2, 2181 },
	{ (Il2CppRGCTXDataType)3, 3204 },
	{ (Il2CppRGCTXDataType)3, 3203 },
	{ (Il2CppRGCTXDataType)3, 3211 },
	{ (Il2CppRGCTXDataType)2, 1134 },
	{ (Il2CppRGCTXDataType)3, 3208 },
	{ (Il2CppRGCTXDataType)3, 8931 },
	{ (Il2CppRGCTXDataType)2, 723 },
	{ (Il2CppRGCTXDataType)3, 2214 },
	{ (Il2CppRGCTXDataType)1, 986 },
	{ (Il2CppRGCTXDataType)2, 2135 },
	{ (Il2CppRGCTXDataType)3, 3207 },
	{ (Il2CppRGCTXDataType)1, 2135 },
	{ (Il2CppRGCTXDataType)1, 1134 },
	{ (Il2CppRGCTXDataType)2, 2181 },
	{ (Il2CppRGCTXDataType)2, 2135 },
	{ (Il2CppRGCTXDataType)2, 1025 },
	{ (Il2CppRGCTXDataType)2, 1079 },
	{ (Il2CppRGCTXDataType)3, 3212 },
	{ (Il2CppRGCTXDataType)3, 3209 },
	{ (Il2CppRGCTXDataType)3, 3215 },
	{ (Il2CppRGCTXDataType)2, 244 },
	{ (Il2CppRGCTXDataType)3, 2224 },
	{ (Il2CppRGCTXDataType)2, 346 },
};
extern const CustomAttributesCacheGenerator g_System_Core_AttributeGenerators[];
IL2CPP_EXTERN_C const Il2CppCodeGenModule g_System_Core_CodeGenModule;
const Il2CppCodeGenModule g_System_Core_CodeGenModule = 
{
	"System.Core.dll",
	143,
	s_methodPointers,
	0,
	NULL,
	s_InvokerIndices,
	0,
	NULL,
	44,
	s_rgctxIndices,
	231,
	s_rgctxValues,
	NULL,
	g_System_Core_AttributeGenerators,
	NULL, // module initializer,
	NULL,
	NULL,
	NULL,
};
