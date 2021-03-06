﻿// Copyright (c) 2008-2018, Hazelcast, Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Hazelcast.Client;

namespace Hazelcast.Examples.Org.Website.Samples
{
    public class AtomicLongSample
    {
        public static void Run(string[] args)
        {
            // Start the Hazelcast Client and connect to an already running Hazelcast Cluster on 127.0.0.1
            var hz = HazelcastClient.NewHazelcastClient();
            // Get an Atomic Counter, we'll call it "counter"
            var counter = hz.GetAtomicLong("counter");
            // Add and Get the "counter"
            counter.AddAndGet(3); // value is now 3
            // Display the "counter" value
            Console.WriteLine("counter: " + counter.Get());
            // Shutdown this Hazelcast Client
            hz.Shutdown();
        }
    }
}