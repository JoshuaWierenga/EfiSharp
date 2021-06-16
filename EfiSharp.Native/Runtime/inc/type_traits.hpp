// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.
//
// type_traits.hpp
//
// Type trait metaprogramming utilities.
//

#ifndef __TYPE_TRAITS_HPP__
#define __TYPE_TRAITS_HPP__

#include <stdint.h>

namespace type_traits
{
	////////////////////////////////////////////////////////////////////////////////
// Remove const qualifications, if any. Access using remove_const::type
//
template <typename T> struct remove_const { typedef T type; };
template <typename T> struct remove_const<T const> { typedef T type; };
}

#endif