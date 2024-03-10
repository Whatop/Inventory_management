#include "pch-c.h"
#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include "codegen/il2cpp-codegen-metadata.h"





// 0x00000001 System.Exception System.Linq.Error::ArgumentNull(System.String)
extern void Error_ArgumentNull_m0EDA0D46D72CA692518E3E2EB75B48044D8FD41E (void);
// 0x00000002 System.Exception System.Linq.Error::ArgumentOutOfRange(System.String)
extern void Error_ArgumentOutOfRange_m2EFB999454161A6B48F8DAC3753FDC190538F0F2 (void);
// 0x00000003 System.Exception System.Linq.Error::MoreThanOneMatch()
extern void Error_MoreThanOneMatch_m4C4756AF34A76EF12F3B2B6D8C78DE547F0FBCF8 (void);
// 0x00000004 System.Exception System.Linq.Error::NoElements()
extern void Error_NoElements_mB89E91246572F009281D79730950808F17C3F353 (void);
// 0x00000005 System.Exception System.Linq.Error::NotSupported()
extern void Error_NotSupported_m51A0560ABF374B66CF6D1208DAF27C4CBAD9AABA (void);
// 0x00000006 System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable::Where(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x00000007 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable::Select(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TResult>)
// 0x00000008 System.Func`2<TSource,System.Boolean> System.Linq.Enumerable::CombinePredicates(System.Func`2<TSource,System.Boolean>,System.Func`2<TSource,System.Boolean>)
// 0x00000009 System.Func`2<TSource,TResult> System.Linq.Enumerable::CombineSelectors(System.Func`2<TSource,TMiddle>,System.Func`2<TMiddle,TResult>)
// 0x0000000A System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable::SelectMany(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Collections.Generic.IEnumerable`1<TCollection>>,System.Func`3<TSource,TCollection,TResult>)
// 0x0000000B System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable::SelectManyIterator(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Collections.Generic.IEnumerable`1<TCollection>>,System.Func`3<TSource,TCollection,TResult>)
// 0x0000000C System.Linq.IOrderedEnumerable`1<TSource> System.Linq.Enumerable::OrderBy(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TKey>)
// 0x0000000D System.Linq.IOrderedEnumerable`1<TSource> System.Linq.Enumerable::ThenBy(System.Linq.IOrderedEnumerable`1<TSource>,System.Func`2<TSource,TKey>)
// 0x0000000E System.Collections.Generic.IEnumerable`1<System.Linq.IGrouping`2<TKey,TSource>> System.Linq.Enumerable::GroupBy(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TKey>)
// 0x0000000F System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable::Distinct(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x00000010 System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable::DistinctIterator(System.Collections.Generic.IEnumerable`1<TSource>,System.Collections.Generic.IEqualityComparer`1<TSource>)
// 0x00000011 TSource[] System.Linq.Enumerable::ToArray(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x00000012 System.Collections.Generic.List`1<TSource> System.Linq.Enumerable::ToList(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x00000013 TSource System.Linq.Enumerable::First(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x00000014 TSource System.Linq.Enumerable::Last(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x00000015 TSource System.Linq.Enumerable::SingleOrDefault(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x00000016 TSource System.Linq.Enumerable::ElementAt(System.Collections.Generic.IEnumerable`1<TSource>,System.Int32)
// 0x00000017 System.Boolean System.Linq.Enumerable::Any(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x00000018 System.Boolean System.Linq.Enumerable::Any(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x00000019 System.Int32 System.Linq.Enumerable::Count(System.Collections.Generic.IEnumerable`1<TSource>)
// 0x0000001A System.Boolean System.Linq.Enumerable::Contains(System.Collections.Generic.IEnumerable`1<TSource>,TSource)
// 0x0000001B System.Boolean System.Linq.Enumerable::Contains(System.Collections.Generic.IEnumerable`1<TSource>,TSource,System.Collections.Generic.IEqualityComparer`1<TSource>)
// 0x0000001C System.Void System.Linq.Enumerable/Iterator`1::.ctor()
// 0x0000001D TSource System.Linq.Enumerable/Iterator`1::get_Current()
// 0x0000001E System.Linq.Enumerable/Iterator`1<TSource> System.Linq.Enumerable/Iterator`1::Clone()
// 0x0000001F System.Void System.Linq.Enumerable/Iterator`1::Dispose()
// 0x00000020 System.Collections.Generic.IEnumerator`1<TSource> System.Linq.Enumerable/Iterator`1::GetEnumerator()
// 0x00000021 System.Boolean System.Linq.Enumerable/Iterator`1::MoveNext()
// 0x00000022 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable/Iterator`1::Select(System.Func`2<TSource,TResult>)
// 0x00000023 System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable/Iterator`1::Where(System.Func`2<TSource,System.Boolean>)
// 0x00000024 System.Object System.Linq.Enumerable/Iterator`1::System.Collections.IEnumerator.get_Current()
// 0x00000025 System.Collections.IEnumerator System.Linq.Enumerable/Iterator`1::System.Collections.IEnumerable.GetEnumerator()
// 0x00000026 System.Void System.Linq.Enumerable/Iterator`1::System.Collections.IEnumerator.Reset()
// 0x00000027 System.Void System.Linq.Enumerable/WhereEnumerableIterator`1::.ctor(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x00000028 System.Linq.Enumerable/Iterator`1<TSource> System.Linq.Enumerable/WhereEnumerableIterator`1::Clone()
// 0x00000029 System.Void System.Linq.Enumerable/WhereEnumerableIterator`1::Dispose()
// 0x0000002A System.Boolean System.Linq.Enumerable/WhereEnumerableIterator`1::MoveNext()
// 0x0000002B System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable/WhereEnumerableIterator`1::Select(System.Func`2<TSource,TResult>)
// 0x0000002C System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable/WhereEnumerableIterator`1::Where(System.Func`2<TSource,System.Boolean>)
// 0x0000002D System.Void System.Linq.Enumerable/WhereArrayIterator`1::.ctor(TSource[],System.Func`2<TSource,System.Boolean>)
// 0x0000002E System.Linq.Enumerable/Iterator`1<TSource> System.Linq.Enumerable/WhereArrayIterator`1::Clone()
// 0x0000002F System.Boolean System.Linq.Enumerable/WhereArrayIterator`1::MoveNext()
// 0x00000030 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable/WhereArrayIterator`1::Select(System.Func`2<TSource,TResult>)
// 0x00000031 System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable/WhereArrayIterator`1::Where(System.Func`2<TSource,System.Boolean>)
// 0x00000032 System.Void System.Linq.Enumerable/WhereListIterator`1::.ctor(System.Collections.Generic.List`1<TSource>,System.Func`2<TSource,System.Boolean>)
// 0x00000033 System.Linq.Enumerable/Iterator`1<TSource> System.Linq.Enumerable/WhereListIterator`1::Clone()
// 0x00000034 System.Boolean System.Linq.Enumerable/WhereListIterator`1::MoveNext()
// 0x00000035 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable/WhereListIterator`1::Select(System.Func`2<TSource,TResult>)
// 0x00000036 System.Collections.Generic.IEnumerable`1<TSource> System.Linq.Enumerable/WhereListIterator`1::Where(System.Func`2<TSource,System.Boolean>)
// 0x00000037 System.Void System.Linq.Enumerable/WhereSelectEnumerableIterator`2::.ctor(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,System.Boolean>,System.Func`2<TSource,TResult>)
// 0x00000038 System.Linq.Enumerable/Iterator`1<TResult> System.Linq.Enumerable/WhereSelectEnumerableIterator`2::Clone()
// 0x00000039 System.Void System.Linq.Enumerable/WhereSelectEnumerableIterator`2::Dispose()
// 0x0000003A System.Boolean System.Linq.Enumerable/WhereSelectEnumerableIterator`2::MoveNext()
// 0x0000003B System.Collections.Generic.IEnumerable`1<TResult2> System.Linq.Enumerable/WhereSelectEnumerableIterator`2::Select(System.Func`2<TResult,TResult2>)
// 0x0000003C System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable/WhereSelectEnumerableIterator`2::Where(System.Func`2<TResult,System.Boolean>)
// 0x0000003D System.Void System.Linq.Enumerable/WhereSelectArrayIterator`2::.ctor(TSource[],System.Func`2<TSource,System.Boolean>,System.Func`2<TSource,TResult>)
// 0x0000003E System.Linq.Enumerable/Iterator`1<TResult> System.Linq.Enumerable/WhereSelectArrayIterator`2::Clone()
// 0x0000003F System.Boolean System.Linq.Enumerable/WhereSelectArrayIterator`2::MoveNext()
// 0x00000040 System.Collections.Generic.IEnumerable`1<TResult2> System.Linq.Enumerable/WhereSelectArrayIterator`2::Select(System.Func`2<TResult,TResult2>)
// 0x00000041 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable/WhereSelectArrayIterator`2::Where(System.Func`2<TResult,System.Boolean>)
// 0x00000042 System.Void System.Linq.Enumerable/WhereSelectListIterator`2::.ctor(System.Collections.Generic.List`1<TSource>,System.Func`2<TSource,System.Boolean>,System.Func`2<TSource,TResult>)
// 0x00000043 System.Linq.Enumerable/Iterator`1<TResult> System.Linq.Enumerable/WhereSelectListIterator`2::Clone()
// 0x00000044 System.Boolean System.Linq.Enumerable/WhereSelectListIterator`2::MoveNext()
// 0x00000045 System.Collections.Generic.IEnumerable`1<TResult2> System.Linq.Enumerable/WhereSelectListIterator`2::Select(System.Func`2<TResult,TResult2>)
// 0x00000046 System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable/WhereSelectListIterator`2::Where(System.Func`2<TResult,System.Boolean>)
// 0x00000047 System.Void System.Linq.Enumerable/<>c__DisplayClass6_0`1::.ctor()
// 0x00000048 System.Boolean System.Linq.Enumerable/<>c__DisplayClass6_0`1::<CombinePredicates>b__0(TSource)
// 0x00000049 System.Void System.Linq.Enumerable/<>c__DisplayClass7_0`3::.ctor()
// 0x0000004A TResult System.Linq.Enumerable/<>c__DisplayClass7_0`3::<CombineSelectors>b__0(TSource)
// 0x0000004B System.Void System.Linq.Enumerable/<SelectManyIterator>d__23`3::.ctor(System.Int32)
// 0x0000004C System.Void System.Linq.Enumerable/<SelectManyIterator>d__23`3::System.IDisposable.Dispose()
// 0x0000004D System.Boolean System.Linq.Enumerable/<SelectManyIterator>d__23`3::MoveNext()
// 0x0000004E System.Void System.Linq.Enumerable/<SelectManyIterator>d__23`3::<>m__Finally1()
// 0x0000004F System.Void System.Linq.Enumerable/<SelectManyIterator>d__23`3::<>m__Finally2()
// 0x00000050 TResult System.Linq.Enumerable/<SelectManyIterator>d__23`3::System.Collections.Generic.IEnumerator<TResult>.get_Current()
// 0x00000051 System.Void System.Linq.Enumerable/<SelectManyIterator>d__23`3::System.Collections.IEnumerator.Reset()
// 0x00000052 System.Object System.Linq.Enumerable/<SelectManyIterator>d__23`3::System.Collections.IEnumerator.get_Current()
// 0x00000053 System.Collections.Generic.IEnumerator`1<TResult> System.Linq.Enumerable/<SelectManyIterator>d__23`3::System.Collections.Generic.IEnumerable<TResult>.GetEnumerator()
// 0x00000054 System.Collections.IEnumerator System.Linq.Enumerable/<SelectManyIterator>d__23`3::System.Collections.IEnumerable.GetEnumerator()
// 0x00000055 System.Void System.Linq.Enumerable/<DistinctIterator>d__68`1::.ctor(System.Int32)
// 0x00000056 System.Void System.Linq.Enumerable/<DistinctIterator>d__68`1::System.IDisposable.Dispose()
// 0x00000057 System.Boolean System.Linq.Enumerable/<DistinctIterator>d__68`1::MoveNext()
// 0x00000058 System.Void System.Linq.Enumerable/<DistinctIterator>d__68`1::<>m__Finally1()
// 0x00000059 TSource System.Linq.Enumerable/<DistinctIterator>d__68`1::System.Collections.Generic.IEnumerator<TSource>.get_Current()
// 0x0000005A System.Void System.Linq.Enumerable/<DistinctIterator>d__68`1::System.Collections.IEnumerator.Reset()
// 0x0000005B System.Object System.Linq.Enumerable/<DistinctIterator>d__68`1::System.Collections.IEnumerator.get_Current()
// 0x0000005C System.Collections.Generic.IEnumerator`1<TSource> System.Linq.Enumerable/<DistinctIterator>d__68`1::System.Collections.Generic.IEnumerable<TSource>.GetEnumerator()
// 0x0000005D System.Collections.IEnumerator System.Linq.Enumerable/<DistinctIterator>d__68`1::System.Collections.IEnumerable.GetEnumerator()
// 0x0000005E System.Func`2<TElement,TElement> System.Linq.IdentityFunction`1::get_Instance()
// 0x0000005F System.Void System.Linq.IdentityFunction`1/<>c::.cctor()
// 0x00000060 System.Void System.Linq.IdentityFunction`1/<>c::.ctor()
// 0x00000061 TElement System.Linq.IdentityFunction`1/<>c::<get_Instance>b__1_0(TElement)
// 0x00000062 System.Linq.IOrderedEnumerable`1<TElement> System.Linq.IOrderedEnumerable`1::CreateOrderedEnumerable(System.Func`2<TElement,TKey>,System.Collections.Generic.IComparer`1<TKey>,System.Boolean)
// 0x00000063 System.Linq.Lookup`2<TKey,TElement> System.Linq.Lookup`2::Create(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TKey>,System.Func`2<TSource,TElement>,System.Collections.Generic.IEqualityComparer`1<TKey>)
// 0x00000064 System.Void System.Linq.Lookup`2::.ctor(System.Collections.Generic.IEqualityComparer`1<TKey>)
// 0x00000065 System.Collections.Generic.IEnumerator`1<System.Linq.IGrouping`2<TKey,TElement>> System.Linq.Lookup`2::GetEnumerator()
// 0x00000066 System.Collections.IEnumerator System.Linq.Lookup`2::System.Collections.IEnumerable.GetEnumerator()
// 0x00000067 System.Int32 System.Linq.Lookup`2::InternalGetHashCode(TKey)
// 0x00000068 System.Linq.Lookup`2/Grouping<TKey,TElement> System.Linq.Lookup`2::GetGrouping(TKey,System.Boolean)
// 0x00000069 System.Void System.Linq.Lookup`2::Resize()
// 0x0000006A System.Void System.Linq.Lookup`2/Grouping::Add(TElement)
// 0x0000006B System.Collections.Generic.IEnumerator`1<TElement> System.Linq.Lookup`2/Grouping::GetEnumerator()
// 0x0000006C System.Collections.IEnumerator System.Linq.Lookup`2/Grouping::System.Collections.IEnumerable.GetEnumerator()
// 0x0000006D System.Int32 System.Linq.Lookup`2/Grouping::System.Collections.Generic.ICollection<TElement>.get_Count()
// 0x0000006E System.Boolean System.Linq.Lookup`2/Grouping::System.Collections.Generic.ICollection<TElement>.get_IsReadOnly()
// 0x0000006F System.Void System.Linq.Lookup`2/Grouping::System.Collections.Generic.ICollection<TElement>.Add(TElement)
// 0x00000070 System.Void System.Linq.Lookup`2/Grouping::System.Collections.Generic.ICollection<TElement>.Clear()
// 0x00000071 System.Boolean System.Linq.Lookup`2/Grouping::System.Collections.Generic.ICollection<TElement>.Contains(TElement)
// 0x00000072 System.Void System.Linq.Lookup`2/Grouping::System.Collections.Generic.ICollection<TElement>.CopyTo(TElement[],System.Int32)
// 0x00000073 System.Boolean System.Linq.Lookup`2/Grouping::System.Collections.Generic.ICollection<TElement>.Remove(TElement)
// 0x00000074 System.Int32 System.Linq.Lookup`2/Grouping::System.Collections.Generic.IList<TElement>.IndexOf(TElement)
// 0x00000075 System.Void System.Linq.Lookup`2/Grouping::System.Collections.Generic.IList<TElement>.Insert(System.Int32,TElement)
// 0x00000076 System.Void System.Linq.Lookup`2/Grouping::System.Collections.Generic.IList<TElement>.RemoveAt(System.Int32)
// 0x00000077 TElement System.Linq.Lookup`2/Grouping::System.Collections.Generic.IList<TElement>.get_Item(System.Int32)
// 0x00000078 System.Void System.Linq.Lookup`2/Grouping::System.Collections.Generic.IList<TElement>.set_Item(System.Int32,TElement)
// 0x00000079 System.Void System.Linq.Lookup`2/Grouping::.ctor()
// 0x0000007A System.Void System.Linq.Lookup`2/Grouping/<GetEnumerator>d__7::.ctor(System.Int32)
// 0x0000007B System.Void System.Linq.Lookup`2/Grouping/<GetEnumerator>d__7::System.IDisposable.Dispose()
// 0x0000007C System.Boolean System.Linq.Lookup`2/Grouping/<GetEnumerator>d__7::MoveNext()
// 0x0000007D TElement System.Linq.Lookup`2/Grouping/<GetEnumerator>d__7::System.Collections.Generic.IEnumerator<TElement>.get_Current()
// 0x0000007E System.Void System.Linq.Lookup`2/Grouping/<GetEnumerator>d__7::System.Collections.IEnumerator.Reset()
// 0x0000007F System.Object System.Linq.Lookup`2/Grouping/<GetEnumerator>d__7::System.Collections.IEnumerator.get_Current()
// 0x00000080 System.Void System.Linq.Lookup`2/<GetEnumerator>d__12::.ctor(System.Int32)
// 0x00000081 System.Void System.Linq.Lookup`2/<GetEnumerator>d__12::System.IDisposable.Dispose()
// 0x00000082 System.Boolean System.Linq.Lookup`2/<GetEnumerator>d__12::MoveNext()
// 0x00000083 System.Linq.IGrouping`2<TKey,TElement> System.Linq.Lookup`2/<GetEnumerator>d__12::System.Collections.Generic.IEnumerator<System.Linq.IGrouping<TKey,TElement>>.get_Current()
// 0x00000084 System.Void System.Linq.Lookup`2/<GetEnumerator>d__12::System.Collections.IEnumerator.Reset()
// 0x00000085 System.Object System.Linq.Lookup`2/<GetEnumerator>d__12::System.Collections.IEnumerator.get_Current()
// 0x00000086 System.Void System.Linq.Set`1::.ctor(System.Collections.Generic.IEqualityComparer`1<TElement>)
// 0x00000087 System.Boolean System.Linq.Set`1::Add(TElement)
// 0x00000088 System.Boolean System.Linq.Set`1::Find(TElement,System.Boolean)
// 0x00000089 System.Void System.Linq.Set`1::Resize()
// 0x0000008A System.Int32 System.Linq.Set`1::InternalGetHashCode(TElement)
// 0x0000008B System.Void System.Linq.GroupedEnumerable`3::.ctor(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TKey>,System.Func`2<TSource,TElement>,System.Collections.Generic.IEqualityComparer`1<TKey>)
// 0x0000008C System.Collections.Generic.IEnumerator`1<System.Linq.IGrouping`2<TKey,TElement>> System.Linq.GroupedEnumerable`3::GetEnumerator()
// 0x0000008D System.Collections.IEnumerator System.Linq.GroupedEnumerable`3::System.Collections.IEnumerable.GetEnumerator()
// 0x0000008E System.Collections.Generic.IEnumerator`1<TElement> System.Linq.OrderedEnumerable`1::GetEnumerator()
// 0x0000008F System.Linq.EnumerableSorter`1<TElement> System.Linq.OrderedEnumerable`1::GetEnumerableSorter(System.Linq.EnumerableSorter`1<TElement>)
// 0x00000090 System.Collections.IEnumerator System.Linq.OrderedEnumerable`1::System.Collections.IEnumerable.GetEnumerator()
// 0x00000091 System.Linq.IOrderedEnumerable`1<TElement> System.Linq.OrderedEnumerable`1::System.Linq.IOrderedEnumerable<TElement>.CreateOrderedEnumerable(System.Func`2<TElement,TKey>,System.Collections.Generic.IComparer`1<TKey>,System.Boolean)
// 0x00000092 System.Void System.Linq.OrderedEnumerable`1::.ctor()
// 0x00000093 System.Void System.Linq.OrderedEnumerable`1/<GetEnumerator>d__1::.ctor(System.Int32)
// 0x00000094 System.Void System.Linq.OrderedEnumerable`1/<GetEnumerator>d__1::System.IDisposable.Dispose()
// 0x00000095 System.Boolean System.Linq.OrderedEnumerable`1/<GetEnumerator>d__1::MoveNext()
// 0x00000096 TElement System.Linq.OrderedEnumerable`1/<GetEnumerator>d__1::System.Collections.Generic.IEnumerator<TElement>.get_Current()
// 0x00000097 System.Void System.Linq.OrderedEnumerable`1/<GetEnumerator>d__1::System.Collections.IEnumerator.Reset()
// 0x00000098 System.Object System.Linq.OrderedEnumerable`1/<GetEnumerator>d__1::System.Collections.IEnumerator.get_Current()
// 0x00000099 System.Void System.Linq.OrderedEnumerable`2::.ctor(System.Collections.Generic.IEnumerable`1<TElement>,System.Func`2<TElement,TKey>,System.Collections.Generic.IComparer`1<TKey>,System.Boolean)
// 0x0000009A System.Linq.EnumerableSorter`1<TElement> System.Linq.OrderedEnumerable`2::GetEnumerableSorter(System.Linq.EnumerableSorter`1<TElement>)
// 0x0000009B System.Void System.Linq.EnumerableSorter`1::ComputeKeys(TElement[],System.Int32)
// 0x0000009C System.Int32 System.Linq.EnumerableSorter`1::CompareKeys(System.Int32,System.Int32)
// 0x0000009D System.Int32[] System.Linq.EnumerableSorter`1::Sort(TElement[],System.Int32)
// 0x0000009E System.Void System.Linq.EnumerableSorter`1::QuickSort(System.Int32[],System.Int32,System.Int32)
// 0x0000009F System.Void System.Linq.EnumerableSorter`1::.ctor()
// 0x000000A0 System.Void System.Linq.EnumerableSorter`2::.ctor(System.Func`2<TElement,TKey>,System.Collections.Generic.IComparer`1<TKey>,System.Boolean,System.Linq.EnumerableSorter`1<TElement>)
// 0x000000A1 System.Void System.Linq.EnumerableSorter`2::ComputeKeys(TElement[],System.Int32)
// 0x000000A2 System.Int32 System.Linq.EnumerableSorter`2::CompareKeys(System.Int32,System.Int32)
// 0x000000A3 System.Void System.Linq.Buffer`1::.ctor(System.Collections.Generic.IEnumerable`1<TElement>)
// 0x000000A4 TElement[] System.Linq.Buffer`1::ToArray()
// 0x000000A5 System.Void System.Collections.Generic.HashSet`1::.ctor()
// 0x000000A6 System.Void System.Collections.Generic.HashSet`1::.ctor(System.Collections.Generic.IEqualityComparer`1<T>)
// 0x000000A7 System.Void System.Collections.Generic.HashSet`1::.ctor(System.Collections.Generic.IEnumerable`1<T>)
// 0x000000A8 System.Void System.Collections.Generic.HashSet`1::.ctor(System.Collections.Generic.IEnumerable`1<T>,System.Collections.Generic.IEqualityComparer`1<T>)
// 0x000000A9 System.Void System.Collections.Generic.HashSet`1::.ctor(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)
// 0x000000AA System.Void System.Collections.Generic.HashSet`1::CopyFrom(System.Collections.Generic.HashSet`1<T>)
// 0x000000AB System.Void System.Collections.Generic.HashSet`1::System.Collections.Generic.ICollection<T>.Add(T)
// 0x000000AC System.Void System.Collections.Generic.HashSet`1::Clear()
// 0x000000AD System.Boolean System.Collections.Generic.HashSet`1::Contains(T)
// 0x000000AE System.Void System.Collections.Generic.HashSet`1::CopyTo(T[],System.Int32)
// 0x000000AF System.Boolean System.Collections.Generic.HashSet`1::Remove(T)
// 0x000000B0 System.Int32 System.Collections.Generic.HashSet`1::get_Count()
// 0x000000B1 System.Boolean System.Collections.Generic.HashSet`1::System.Collections.Generic.ICollection<T>.get_IsReadOnly()
// 0x000000B2 System.Collections.Generic.HashSet`1/Enumerator<T> System.Collections.Generic.HashSet`1::GetEnumerator()
// 0x000000B3 System.Collections.Generic.IEnumerator`1<T> System.Collections.Generic.HashSet`1::System.Collections.Generic.IEnumerable<T>.GetEnumerator()
// 0x000000B4 System.Collections.IEnumerator System.Collections.Generic.HashSet`1::System.Collections.IEnumerable.GetEnumerator()
// 0x000000B5 System.Void System.Collections.Generic.HashSet`1::GetObjectData(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)
// 0x000000B6 System.Void System.Collections.Generic.HashSet`1::OnDeserialization(System.Object)
// 0x000000B7 System.Boolean System.Collections.Generic.HashSet`1::Add(T)
// 0x000000B8 System.Void System.Collections.Generic.HashSet`1::UnionWith(System.Collections.Generic.IEnumerable`1<T>)
// 0x000000B9 System.Void System.Collections.Generic.HashSet`1::CopyTo(T[])
// 0x000000BA System.Void System.Collections.Generic.HashSet`1::CopyTo(T[],System.Int32,System.Int32)
// 0x000000BB System.Collections.Generic.IEqualityComparer`1<T> System.Collections.Generic.HashSet`1::get_Comparer()
// 0x000000BC System.Void System.Collections.Generic.HashSet`1::TrimExcess()
// 0x000000BD System.Void System.Collections.Generic.HashSet`1::Initialize(System.Int32)
// 0x000000BE System.Void System.Collections.Generic.HashSet`1::IncreaseCapacity()
// 0x000000BF System.Void System.Collections.Generic.HashSet`1::SetCapacity(System.Int32)
// 0x000000C0 System.Boolean System.Collections.Generic.HashSet`1::AddIfNotPresent(T)
// 0x000000C1 System.Void System.Collections.Generic.HashSet`1::AddValue(System.Int32,System.Int32,T)
// 0x000000C2 System.Boolean System.Collections.Generic.HashSet`1::AreEqualityComparersEqual(System.Collections.Generic.HashSet`1<T>,System.Collections.Generic.HashSet`1<T>)
// 0x000000C3 System.Int32 System.Collections.Generic.HashSet`1::InternalGetHashCode(T)
// 0x000000C4 System.Void System.Collections.Generic.HashSet`1/Enumerator::.ctor(System.Collections.Generic.HashSet`1<T>)
// 0x000000C5 System.Void System.Collections.Generic.HashSet`1/Enumerator::Dispose()
// 0x000000C6 System.Boolean System.Collections.Generic.HashSet`1/Enumerator::MoveNext()
// 0x000000C7 T System.Collections.Generic.HashSet`1/Enumerator::get_Current()
// 0x000000C8 System.Object System.Collections.Generic.HashSet`1/Enumerator::System.Collections.IEnumerator.get_Current()
// 0x000000C9 System.Void System.Collections.Generic.HashSet`1/Enumerator::System.Collections.IEnumerator.Reset()
static Il2CppMethodPointer s_methodPointers[201] = 
{
	Error_ArgumentNull_m0EDA0D46D72CA692518E3E2EB75B48044D8FD41E,
	Error_ArgumentOutOfRange_m2EFB999454161A6B48F8DAC3753FDC190538F0F2,
	Error_MoreThanOneMatch_m4C4756AF34A76EF12F3B2B6D8C78DE547F0FBCF8,
	Error_NoElements_mB89E91246572F009281D79730950808F17C3F353,
	Error_NotSupported_m51A0560ABF374B66CF6D1208DAF27C4CBAD9AABA,
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
static const int32_t s_InvokerIndices[201] = 
{
	2379,
	2379,
	2463,
	2463,
	2463,
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
static const Il2CppTokenRangePair s_rgctxIndices[56] = 
{
	{ 0x02000004, { 75, 4 } },
	{ 0x02000005, { 79, 9 } },
	{ 0x02000006, { 90, 7 } },
	{ 0x02000007, { 99, 10 } },
	{ 0x02000008, { 111, 11 } },
	{ 0x02000009, { 125, 9 } },
	{ 0x0200000A, { 137, 12 } },
	{ 0x0200000B, { 152, 1 } },
	{ 0x0200000C, { 153, 2 } },
	{ 0x0200000D, { 155, 13 } },
	{ 0x0200000E, { 168, 11 } },
	{ 0x0200000F, { 179, 4 } },
	{ 0x02000010, { 183, 3 } },
	{ 0x02000013, { 186, 17 } },
	{ 0x02000014, { 207, 5 } },
	{ 0x02000015, { 212, 1 } },
	{ 0x02000017, { 213, 8 } },
	{ 0x02000019, { 221, 4 } },
	{ 0x0200001A, { 225, 3 } },
	{ 0x0200001B, { 230, 5 } },
	{ 0x0200001C, { 235, 7 } },
	{ 0x0200001D, { 242, 3 } },
	{ 0x0200001E, { 245, 7 } },
	{ 0x0200001F, { 252, 4 } },
	{ 0x02000020, { 256, 34 } },
	{ 0x02000022, { 290, 2 } },
	{ 0x06000006, { 0, 10 } },
	{ 0x06000007, { 10, 10 } },
	{ 0x06000008, { 20, 5 } },
	{ 0x06000009, { 25, 5 } },
	{ 0x0600000A, { 30, 1 } },
	{ 0x0600000B, { 31, 2 } },
	{ 0x0600000C, { 33, 2 } },
	{ 0x0600000D, { 35, 1 } },
	{ 0x0600000E, { 36, 4 } },
	{ 0x0600000F, { 40, 1 } },
	{ 0x06000010, { 41, 2 } },
	{ 0x06000011, { 43, 3 } },
	{ 0x06000012, { 46, 2 } },
	{ 0x06000013, { 48, 4 } },
	{ 0x06000014, { 52, 4 } },
	{ 0x06000015, { 56, 3 } },
	{ 0x06000016, { 59, 3 } },
	{ 0x06000017, { 62, 1 } },
	{ 0x06000018, { 63, 3 } },
	{ 0x06000019, { 66, 2 } },
	{ 0x0600001A, { 68, 2 } },
	{ 0x0600001B, { 70, 5 } },
	{ 0x0600002B, { 88, 2 } },
	{ 0x06000030, { 97, 2 } },
	{ 0x06000035, { 109, 2 } },
	{ 0x0600003B, { 122, 3 } },
	{ 0x06000040, { 134, 3 } },
	{ 0x06000045, { 149, 3 } },
	{ 0x06000063, { 203, 4 } },
	{ 0x06000091, { 228, 2 } },
};
static const Il2CppRGCTXDefinition s_rgctxValues[292] = 
{
	{ (Il2CppRGCTXDataType)2, 1439 },
	{ (Il2CppRGCTXDataType)3, 4503 },
	{ (Il2CppRGCTXDataType)2, 2321 },
	{ (Il2CppRGCTXDataType)2, 1979 },
	{ (Il2CppRGCTXDataType)3, 7859 },
	{ (Il2CppRGCTXDataType)2, 1518 },
	{ (Il2CppRGCTXDataType)2, 1986 },
	{ (Il2CppRGCTXDataType)3, 7877 },
	{ (Il2CppRGCTXDataType)2, 1981 },
	{ (Il2CppRGCTXDataType)3, 7866 },
	{ (Il2CppRGCTXDataType)2, 1440 },
	{ (Il2CppRGCTXDataType)3, 4504 },
	{ (Il2CppRGCTXDataType)2, 2344 },
	{ (Il2CppRGCTXDataType)2, 1988 },
	{ (Il2CppRGCTXDataType)3, 7884 },
	{ (Il2CppRGCTXDataType)2, 1545 },
	{ (Il2CppRGCTXDataType)2, 1996 },
	{ (Il2CppRGCTXDataType)3, 7903 },
	{ (Il2CppRGCTXDataType)2, 1992 },
	{ (Il2CppRGCTXDataType)3, 7893 },
	{ (Il2CppRGCTXDataType)2, 534 },
	{ (Il2CppRGCTXDataType)3, 29 },
	{ (Il2CppRGCTXDataType)3, 30 },
	{ (Il2CppRGCTXDataType)2, 906 },
	{ (Il2CppRGCTXDataType)3, 3288 },
	{ (Il2CppRGCTXDataType)2, 535 },
	{ (Il2CppRGCTXDataType)3, 33 },
	{ (Il2CppRGCTXDataType)3, 34 },
	{ (Il2CppRGCTXDataType)2, 917 },
	{ (Il2CppRGCTXDataType)3, 3292 },
	{ (Il2CppRGCTXDataType)3, 9527 },
	{ (Il2CppRGCTXDataType)2, 546 },
	{ (Il2CppRGCTXDataType)3, 96 },
	{ (Il2CppRGCTXDataType)2, 1785 },
	{ (Il2CppRGCTXDataType)3, 6874 },
	{ (Il2CppRGCTXDataType)3, 3683 },
	{ (Il2CppRGCTXDataType)3, 3688 },
	{ (Il2CppRGCTXDataType)2, 1427 },
	{ (Il2CppRGCTXDataType)2, 1017 },
	{ (Il2CppRGCTXDataType)3, 3505 },
	{ (Il2CppRGCTXDataType)3, 9501 },
	{ (Il2CppRGCTXDataType)2, 540 },
	{ (Il2CppRGCTXDataType)3, 54 },
	{ (Il2CppRGCTXDataType)2, 596 },
	{ (Il2CppRGCTXDataType)3, 534 },
	{ (Il2CppRGCTXDataType)3, 535 },
	{ (Il2CppRGCTXDataType)2, 1519 },
	{ (Il2CppRGCTXDataType)3, 4941 },
	{ (Il2CppRGCTXDataType)2, 1379 },
	{ (Il2CppRGCTXDataType)2, 1043 },
	{ (Il2CppRGCTXDataType)2, 1128 },
	{ (Il2CppRGCTXDataType)2, 1215 },
	{ (Il2CppRGCTXDataType)2, 1380 },
	{ (Il2CppRGCTXDataType)2, 1044 },
	{ (Il2CppRGCTXDataType)2, 1129 },
	{ (Il2CppRGCTXDataType)2, 1216 },
	{ (Il2CppRGCTXDataType)2, 1130 },
	{ (Il2CppRGCTXDataType)2, 1217 },
	{ (Il2CppRGCTXDataType)3, 3289 },
	{ (Il2CppRGCTXDataType)2, 1378 },
	{ (Il2CppRGCTXDataType)2, 1127 },
	{ (Il2CppRGCTXDataType)2, 1214 },
	{ (Il2CppRGCTXDataType)2, 1118 },
	{ (Il2CppRGCTXDataType)2, 1119 },
	{ (Il2CppRGCTXDataType)2, 1212 },
	{ (Il2CppRGCTXDataType)3, 3287 },
	{ (Il2CppRGCTXDataType)2, 1042 },
	{ (Il2CppRGCTXDataType)2, 1126 },
	{ (Il2CppRGCTXDataType)2, 1041 },
	{ (Il2CppRGCTXDataType)3, 9493 },
	{ (Il2CppRGCTXDataType)3, 2914 },
	{ (Il2CppRGCTXDataType)2, 814 },
	{ (Il2CppRGCTXDataType)2, 1121 },
	{ (Il2CppRGCTXDataType)2, 1213 },
	{ (Il2CppRGCTXDataType)2, 1279 },
	{ (Il2CppRGCTXDataType)3, 4505 },
	{ (Il2CppRGCTXDataType)3, 4507 },
	{ (Il2CppRGCTXDataType)2, 368 },
	{ (Il2CppRGCTXDataType)3, 4506 },
	{ (Il2CppRGCTXDataType)3, 4515 },
	{ (Il2CppRGCTXDataType)2, 1443 },
	{ (Il2CppRGCTXDataType)2, 1982 },
	{ (Il2CppRGCTXDataType)3, 7867 },
	{ (Il2CppRGCTXDataType)3, 4516 },
	{ (Il2CppRGCTXDataType)2, 1175 },
	{ (Il2CppRGCTXDataType)2, 1241 },
	{ (Il2CppRGCTXDataType)3, 3300 },
	{ (Il2CppRGCTXDataType)3, 9484 },
	{ (Il2CppRGCTXDataType)2, 1993 },
	{ (Il2CppRGCTXDataType)3, 7894 },
	{ (Il2CppRGCTXDataType)3, 4508 },
	{ (Il2CppRGCTXDataType)2, 1442 },
	{ (Il2CppRGCTXDataType)2, 1980 },
	{ (Il2CppRGCTXDataType)3, 7860 },
	{ (Il2CppRGCTXDataType)3, 3299 },
	{ (Il2CppRGCTXDataType)3, 4509 },
	{ (Il2CppRGCTXDataType)3, 9483 },
	{ (Il2CppRGCTXDataType)2, 1989 },
	{ (Il2CppRGCTXDataType)3, 7885 },
	{ (Il2CppRGCTXDataType)3, 4522 },
	{ (Il2CppRGCTXDataType)2, 1444 },
	{ (Il2CppRGCTXDataType)2, 1987 },
	{ (Il2CppRGCTXDataType)3, 7878 },
	{ (Il2CppRGCTXDataType)3, 4995 },
	{ (Il2CppRGCTXDataType)3, 2378 },
	{ (Il2CppRGCTXDataType)3, 3301 },
	{ (Il2CppRGCTXDataType)3, 2377 },
	{ (Il2CppRGCTXDataType)3, 4523 },
	{ (Il2CppRGCTXDataType)3, 9485 },
	{ (Il2CppRGCTXDataType)2, 1997 },
	{ (Il2CppRGCTXDataType)3, 7904 },
	{ (Il2CppRGCTXDataType)3, 4536 },
	{ (Il2CppRGCTXDataType)2, 1446 },
	{ (Il2CppRGCTXDataType)2, 1995 },
	{ (Il2CppRGCTXDataType)3, 7896 },
	{ (Il2CppRGCTXDataType)3, 4537 },
	{ (Il2CppRGCTXDataType)2, 1178 },
	{ (Il2CppRGCTXDataType)2, 1244 },
	{ (Il2CppRGCTXDataType)3, 3305 },
	{ (Il2CppRGCTXDataType)3, 3304 },
	{ (Il2CppRGCTXDataType)2, 1984 },
	{ (Il2CppRGCTXDataType)3, 7869 },
	{ (Il2CppRGCTXDataType)3, 9488 },
	{ (Il2CppRGCTXDataType)2, 1994 },
	{ (Il2CppRGCTXDataType)3, 7895 },
	{ (Il2CppRGCTXDataType)3, 4529 },
	{ (Il2CppRGCTXDataType)2, 1445 },
	{ (Il2CppRGCTXDataType)2, 1991 },
	{ (Il2CppRGCTXDataType)3, 7887 },
	{ (Il2CppRGCTXDataType)3, 3303 },
	{ (Il2CppRGCTXDataType)3, 3302 },
	{ (Il2CppRGCTXDataType)3, 4530 },
	{ (Il2CppRGCTXDataType)2, 1983 },
	{ (Il2CppRGCTXDataType)3, 7868 },
	{ (Il2CppRGCTXDataType)3, 9487 },
	{ (Il2CppRGCTXDataType)2, 1990 },
	{ (Il2CppRGCTXDataType)3, 7886 },
	{ (Il2CppRGCTXDataType)3, 4543 },
	{ (Il2CppRGCTXDataType)2, 1447 },
	{ (Il2CppRGCTXDataType)2, 1999 },
	{ (Il2CppRGCTXDataType)3, 7906 },
	{ (Il2CppRGCTXDataType)3, 4996 },
	{ (Il2CppRGCTXDataType)3, 2380 },
	{ (Il2CppRGCTXDataType)3, 3307 },
	{ (Il2CppRGCTXDataType)3, 3306 },
	{ (Il2CppRGCTXDataType)3, 2379 },
	{ (Il2CppRGCTXDataType)3, 4544 },
	{ (Il2CppRGCTXDataType)2, 1985 },
	{ (Il2CppRGCTXDataType)3, 7870 },
	{ (Il2CppRGCTXDataType)3, 9489 },
	{ (Il2CppRGCTXDataType)2, 1998 },
	{ (Il2CppRGCTXDataType)3, 7905 },
	{ (Il2CppRGCTXDataType)3, 3296 },
	{ (Il2CppRGCTXDataType)3, 3297 },
	{ (Il2CppRGCTXDataType)3, 3308 },
	{ (Il2CppRGCTXDataType)3, 99 },
	{ (Il2CppRGCTXDataType)3, 98 },
	{ (Il2CppRGCTXDataType)2, 1170 },
	{ (Il2CppRGCTXDataType)2, 1237 },
	{ (Il2CppRGCTXDataType)3, 3298 },
	{ (Il2CppRGCTXDataType)2, 1185 },
	{ (Il2CppRGCTXDataType)2, 1255 },
	{ (Il2CppRGCTXDataType)3, 3356 },
	{ (Il2CppRGCTXDataType)3, 101 },
	{ (Il2CppRGCTXDataType)2, 514 },
	{ (Il2CppRGCTXDataType)2, 547 },
	{ (Il2CppRGCTXDataType)3, 97 },
	{ (Il2CppRGCTXDataType)3, 100 },
	{ (Il2CppRGCTXDataType)3, 56 },
	{ (Il2CppRGCTXDataType)2, 1832 },
	{ (Il2CppRGCTXDataType)3, 7115 },
	{ (Il2CppRGCTXDataType)2, 1167 },
	{ (Il2CppRGCTXDataType)2, 1235 },
	{ (Il2CppRGCTXDataType)3, 7116 },
	{ (Il2CppRGCTXDataType)3, 58 },
	{ (Il2CppRGCTXDataType)2, 364 },
	{ (Il2CppRGCTXDataType)2, 541 },
	{ (Il2CppRGCTXDataType)3, 55 },
	{ (Il2CppRGCTXDataType)3, 57 },
	{ (Il2CppRGCTXDataType)2, 522 },
	{ (Il2CppRGCTXDataType)3, 0 },
	{ (Il2CppRGCTXDataType)2, 926 },
	{ (Il2CppRGCTXDataType)3, 3295 },
	{ (Il2CppRGCTXDataType)2, 525 },
	{ (Il2CppRGCTXDataType)3, 3 },
	{ (Il2CppRGCTXDataType)2, 525 },
	{ (Il2CppRGCTXDataType)2, 1740 },
	{ (Il2CppRGCTXDataType)3, 6453 },
	{ (Il2CppRGCTXDataType)3, 6455 },
	{ (Il2CppRGCTXDataType)3, 3511 },
	{ (Il2CppRGCTXDataType)3, 2946 },
	{ (Il2CppRGCTXDataType)2, 828 },
	{ (Il2CppRGCTXDataType)2, 2396 },
	{ (Il2CppRGCTXDataType)2, 543 },
	{ (Il2CppRGCTXDataType)3, 75 },
	{ (Il2CppRGCTXDataType)3, 6454 },
	{ (Il2CppRGCTXDataType)2, 301 },
	{ (Il2CppRGCTXDataType)2, 1291 },
	{ (Il2CppRGCTXDataType)3, 6456 },
	{ (Il2CppRGCTXDataType)3, 6457 },
	{ (Il2CppRGCTXDataType)2, 1018 },
	{ (Il2CppRGCTXDataType)3, 3510 },
	{ (Il2CppRGCTXDataType)2, 2390 },
	{ (Il2CppRGCTXDataType)2, 1132 },
	{ (Il2CppRGCTXDataType)2, 1218 },
	{ (Il2CppRGCTXDataType)3, 3290 },
	{ (Il2CppRGCTXDataType)3, 3291 },
	{ (Il2CppRGCTXDataType)3, 9253 },
	{ (Il2CppRGCTXDataType)2, 545 },
	{ (Il2CppRGCTXDataType)3, 89 },
	{ (Il2CppRGCTXDataType)3, 3512 },
	{ (Il2CppRGCTXDataType)3, 8036 },
	{ (Il2CppRGCTXDataType)2, 493 },
	{ (Il2CppRGCTXDataType)3, 2954 },
	{ (Il2CppRGCTXDataType)2, 831 },
	{ (Il2CppRGCTXDataType)2, 2409 },
	{ (Il2CppRGCTXDataType)3, 7112 },
	{ (Il2CppRGCTXDataType)3, 7113 },
	{ (Il2CppRGCTXDataType)2, 1295 },
	{ (Il2CppRGCTXDataType)3, 7114 },
	{ (Il2CppRGCTXDataType)2, 317 },
	{ (Il2CppRGCTXDataType)3, 6458 },
	{ (Il2CppRGCTXDataType)2, 1742 },
	{ (Il2CppRGCTXDataType)3, 6459 },
	{ (Il2CppRGCTXDataType)3, 3506 },
	{ (Il2CppRGCTXDataType)2, 542 },
	{ (Il2CppRGCTXDataType)3, 68 },
	{ (Il2CppRGCTXDataType)3, 6861 },
	{ (Il2CppRGCTXDataType)2, 1786 },
	{ (Il2CppRGCTXDataType)3, 6875 },
	{ (Il2CppRGCTXDataType)2, 597 },
	{ (Il2CppRGCTXDataType)3, 536 },
	{ (Il2CppRGCTXDataType)3, 6867 },
	{ (Il2CppRGCTXDataType)3, 2356 },
	{ (Il2CppRGCTXDataType)2, 391 },
	{ (Il2CppRGCTXDataType)3, 6862 },
	{ (Il2CppRGCTXDataType)2, 1782 },
	{ (Il2CppRGCTXDataType)3, 565 },
	{ (Il2CppRGCTXDataType)2, 611 },
	{ (Il2CppRGCTXDataType)2, 798 },
	{ (Il2CppRGCTXDataType)3, 2362 },
	{ (Il2CppRGCTXDataType)3, 6863 },
	{ (Il2CppRGCTXDataType)3, 2351 },
	{ (Il2CppRGCTXDataType)3, 2352 },
	{ (Il2CppRGCTXDataType)3, 2350 },
	{ (Il2CppRGCTXDataType)3, 2353 },
	{ (Il2CppRGCTXDataType)2, 794 },
	{ (Il2CppRGCTXDataType)2, 2388 },
	{ (Il2CppRGCTXDataType)3, 3294 },
	{ (Il2CppRGCTXDataType)3, 2355 },
	{ (Il2CppRGCTXDataType)2, 1106 },
	{ (Il2CppRGCTXDataType)3, 2354 },
	{ (Il2CppRGCTXDataType)2, 1045 },
	{ (Il2CppRGCTXDataType)2, 2347 },
	{ (Il2CppRGCTXDataType)2, 1145 },
	{ (Il2CppRGCTXDataType)2, 1219 },
	{ (Il2CppRGCTXDataType)3, 2936 },
	{ (Il2CppRGCTXDataType)2, 824 },
	{ (Il2CppRGCTXDataType)3, 3530 },
	{ (Il2CppRGCTXDataType)3, 3531 },
	{ (Il2CppRGCTXDataType)2, 1024 },
	{ (Il2CppRGCTXDataType)3, 3534 },
	{ (Il2CppRGCTXDataType)2, 1024 },
	{ (Il2CppRGCTXDataType)3, 3535 },
	{ (Il2CppRGCTXDataType)2, 1046 },
	{ (Il2CppRGCTXDataType)3, 3539 },
	{ (Il2CppRGCTXDataType)3, 3543 },
	{ (Il2CppRGCTXDataType)3, 3542 },
	{ (Il2CppRGCTXDataType)2, 2407 },
	{ (Il2CppRGCTXDataType)3, 3533 },
	{ (Il2CppRGCTXDataType)3, 3532 },
	{ (Il2CppRGCTXDataType)3, 3540 },
	{ (Il2CppRGCTXDataType)2, 1288 },
	{ (Il2CppRGCTXDataType)3, 3537 },
	{ (Il2CppRGCTXDataType)3, 9781 },
	{ (Il2CppRGCTXDataType)2, 800 },
	{ (Il2CppRGCTXDataType)3, 2371 },
	{ (Il2CppRGCTXDataType)1, 1103 },
	{ (Il2CppRGCTXDataType)2, 2355 },
	{ (Il2CppRGCTXDataType)3, 3536 },
	{ (Il2CppRGCTXDataType)1, 2355 },
	{ (Il2CppRGCTXDataType)1, 1288 },
	{ (Il2CppRGCTXDataType)2, 2407 },
	{ (Il2CppRGCTXDataType)2, 2355 },
	{ (Il2CppRGCTXDataType)2, 1149 },
	{ (Il2CppRGCTXDataType)2, 1221 },
	{ (Il2CppRGCTXDataType)3, 3541 },
	{ (Il2CppRGCTXDataType)3, 3538 },
	{ (Il2CppRGCTXDataType)3, 3544 },
	{ (Il2CppRGCTXDataType)2, 271 },
	{ (Il2CppRGCTXDataType)3, 2381 },
	{ (Il2CppRGCTXDataType)2, 377 },
};
extern const CustomAttributesCacheGenerator g_System_Core_AttributeGenerators[];
IL2CPP_EXTERN_C const Il2CppCodeGenModule g_System_Core_CodeGenModule;
const Il2CppCodeGenModule g_System_Core_CodeGenModule = 
{
	"System.Core.dll",
	201,
	s_methodPointers,
	0,
	NULL,
	s_InvokerIndices,
	0,
	NULL,
	56,
	s_rgctxIndices,
	292,
	s_rgctxValues,
	NULL,
	g_System_Core_AttributeGenerators,
	NULL, // module initializer,
	NULL,
	NULL,
	NULL,
};
