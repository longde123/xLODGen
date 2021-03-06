﻿using System;
using System.IO;
using System.Collections.Generic;

namespace LODGenerator.NifMain
{
    public class BSMultiBound : NiObject
    {
        protected int data;

        public BSMultiBound()
        {
            this.data = -1;
        }

        public override void Read(NiHeader header, BinaryReader reader)
        {
            base.Read(header, reader);
            this.data = reader.ReadInt32();
        }

        public override void Write(NiHeader header, BinaryWriter writer)
        {
            List<int> blockReferences = header.GetBlockReferences();
            if (blockReferences.Count > 0)
            {
                if (this.data != -1)
                {
                    this.data = blockReferences[this.data];
                }
            }
            base.Write(header, writer);
            writer.Write(this.data);
        }

        public override uint GetSize(NiHeader header)
        {
            return base.GetSize(header) + 4U;
        }

        public override string GetClassName()
        {
            return "BSMultiBound";
        }

        public int GetData()
        {
            return this.data;
        }

        public void SetData(int value)
        {
            this.data = value;
        }

        public override bool IsDerivedType(string type)
        {
            bool flag = base.IsDerivedType(type);
            if (!flag)
                flag = type == "BSMultiBound";
            return flag;
        }
    }
}
