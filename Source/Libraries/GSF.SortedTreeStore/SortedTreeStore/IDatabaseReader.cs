﻿//******************************************************************************************************
//  IDatabaseReader`2.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  3/1/2014 - Steven E. Chisholm
//       Generated original version of source code. 
//
//******************************************************************************************************

using System;
using GSF.SortedTreeStore.Engine.Reader;
using GSF.SortedTreeStore.Filters;
using GSF.SortedTreeStore.Tree;

namespace GSF.SortedTreeStore
{

    /// <summary>
    /// An interface that is necessary for many of the transformations inside the SortedTreeStore to function.
    /// </summary>
    /// <typeparam name="TKey">A key type supported by the SortedTreeStore</typeparam>
    /// <typeparam name="TValue">A value type supported by the SortedTreeStore</typeparam>
    public interface IDatabaseReader<TKey, TValue> : IDisposable
        where TKey : SortedTreeTypeBase<TKey>, new()
        where TValue : SortedTreeTypeBase<TValue>, new()
    {
        /// <summary>
        /// Reads data from the SortedTreeEngine with the provided read options and server side filters.
        /// </summary>
        /// <param name="readerOptions"></param>
        /// <param name="keySeekFilter"></param>
        /// <param name="keyMatchFilter"></param>
        /// <returns></returns>
        TreeStream<TKey, TValue> Read(SortedTreeEngineReaderOptions readerOptions, SeekFilterBase<TKey> keySeekFilter, MatchFilterBase<TKey, TValue> keyMatchFilter);

    }
}