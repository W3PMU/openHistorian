﻿//******************************************************************************************************
//  TreeHeader.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
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
//  2/22/2012 - Steven E. Chisholm
//       Generated original version of source code. 
//     
//******************************************************************************************************

using System;

namespace openHistorian.Core.StorageSystem.Generic
{
    public partial class BPlusTree<TKey, TValue>
    {
        public static Guid FileType = new Guid("{7bfa9083-701e-4596-8273-8680a739271d}");
        public BinaryStream Stream { get; protected set; }
        public int BlockSize { get; protected set; }
        public uint RootIndexAddress { get; protected set; }
        public byte RootIndexLevel { get; protected set; }
        public int MaximumLeafNodeChildren { get; protected set; }
        public int MaximumInternalNodeChildren { get; protected set; }
        protected long NextUnallocatedByte { get; set; }

        public void TreeHeader(BinaryStream stream)
        {
            Load(stream);
        }

        public void TreeHeader(BinaryStream stream, int blockSize)
        {
            Stream = stream;
            BlockSize = blockSize;
            MaximumLeafNodeChildren = LeafNodeCalculateMaximumChildren(blockSize);
            MaximumInternalNodeChildren = InternalNodeCalculateMaximumChildren(blockSize);
            NextUnallocatedByte = blockSize;
            RootIndexAddress = LeafNodeCreateEmptyNode();
            RootIndexLevel = 0;
            Save(stream);
            Load(stream);
        }
        public void Load(BinaryStream stream)
        {
            Stream = stream;
            Stream.Position = 0;
            if (FileType != stream.ReadGuid())
                throw new Exception("Header Corrupt");
            if (Stream.ReadByte() != 0)
                throw new Exception("Header Corrupt");
            NextUnallocatedByte = stream.ReadInt64();
            BlockSize = stream.ReadInt32();
            MaximumLeafNodeChildren = LeafNodeCalculateMaximumChildren(BlockSize);
            MaximumInternalNodeChildren = InternalNodeCalculateMaximumChildren(BlockSize);
            RootIndexAddress = stream.ReadUInt32();
            RootIndexLevel = stream.ReadByte();
        }

        public void Save(BinaryStream stream)
        {
            stream.Position = 0;
            stream.Write(FileType);
            stream.Write((byte)0); //Version
            stream.Write(NextUnallocatedByte);
            stream.Write(BlockSize);
            stream.Write(RootIndexAddress); //Root Index
            stream.Write(RootIndexLevel); //Root Index
        }

        /// <summary>
        /// Returns the node index address for a freshly allocated block.
        /// The node address is block alligned.
        /// </summary>
        /// <returns></returns>
        public uint AllocateNewNode()
        {
            //Rounds up to the nearest block boundry 
            long byteFragment = NextUnallocatedByte % BlockSize;
            if (byteFragment != 0)
                NextUnallocatedByte += BlockSize - byteFragment;

            uint newBlock = (uint)(NextUnallocatedByte / BlockSize);
            NextUnallocatedByte += BlockSize;
            return newBlock;
        }

        /// <summary>
        /// returns the address for freshly allocated space.
        /// </summary>
        /// <param name="space">the number of bytes to allocate.</param>
        /// <returns></returns>
        public long AllocateSpace(int space)
        {
            long newAddress = NextUnallocatedByte;
            NextUnallocatedByte += space;
            return newAddress;
        }
        
        /// <summary>
        /// This will modify the BinaryStream.Position property.
        /// </summary>
        /// <param name="nodeIndex">the node index to go to</param>
        public void NavigateToNode(uint nodeIndex)
        {
            Stream.Position = nodeIndex * BlockSize;
        }

        public void NavigateToNode(uint nodeIndex, int offset)
        {
            Stream.Position = nodeIndex * BlockSize + offset;
        }

        public void SetRootIndex(uint nodeIndex, byte level)
        {
            RootIndexAddress = nodeIndex;
            RootIndexLevel = level;
        }


    }
}
