# DoctorFlox
## Introduction
DoctorFlox is a lightweight MVVM library for .NET. It is taking a lot of code and ideas from [MVVM Light Toolkit](http://www.mvvmlight.net/) by Laurant Bugnion. We also took ideas and code from [Prism](https://github.com/PrismLibrary/Prism) because they told us that WPF is not in their focus anymore (see this [Issue](https://github.com/PrismLibrary/Prism/issues/1211)).

We polished the source code and got rid of the Silverlight and DotnetFx components. We developed the logic targetting .NET Standard 2.0 which means, that we currently support the following targets in principle:

- .NET Frameworkwork 4.6.1+
- .NET Core 2.0+
- Mono 5.4+
- Xamarin.iOS 10.14+
- UWP 10.0.16299+

DoctorFlox needs references to certain libs which currently means that the project is targetting WPF-projects only!

## DoctorFlox?

We are a bunch of Geeks and so Star Trek is kind of a natural source for project names to us. This beeing said we decided to use Dr. Phlox as our inspiration. He is always patient and looks to ordinary humans just like we do some times :-). Anyway! In order to not conflict with some weird search engine issues (you know what we mean) we decided to use a `F` instead of the `Ph`. In fact it doesn't matter in our opinion because package names like `SlowCheetah` worked out very well.

## Installation

DoctorFlox is available on [NuGet](https://www.nuget.org/packages/devdeer.DoctorFlox). Using NuGet tools just type

    install-package devdeer.DoctorFlox

## More Info

Be sure to check out our [WiKi](https://github.com/devdeer/DoctorFlox/wiki) for more information.

The DoctorFlox logo was designed by [Martin Dahms](http://martin-dahms.de).
