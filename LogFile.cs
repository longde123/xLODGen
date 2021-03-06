﻿using LODGenerator.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace LODGenerator
{
    public class LogFile
    {
        private StreamWriter logWriter;
        private List<string> lineBuffer;
        private bool keepRunning;
        private Thread logThread;

        public LogFile(string filename)
        {
            this.keepRunning = true;
            bool append = true;

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
                this.logWriter = new StreamWriter(filename, append);
            }
            catch
            {
                try
                {
                    this.logWriter = new StreamWriter(Directory.GetCurrentDirectory() + "\\LODGen_log.txt", append);
                }
                catch
                {
                    Console.WriteLine("Can not write to " + Directory.GetCurrentDirectory() + "\\LODGen_log.txt");
                    System.Environment.Exit(403);
                }
            }

            this.logWriter.WriteLine(this.WriteToScreen("============================================================"));
            this.logWriter.WriteLine(this.WriteToScreen("Skyrim Object LOD Generator v1.0.5"));
            this.logWriter.WriteLine(this.WriteToScreen("Created by Ehamloptiran and Zilav"));
            this.logWriter.WriteLine(this.WriteToScreen("Updated by Sheson\n"));
            this.logWriter.WriteLine(this.WriteToScreen("Log started at " + DateTime.Now.ToLongTimeString()));
            this.lineBuffer = new List<string>();
            this.logThread = new Thread((ParameterizedThreadStart)(state =>
            {
                while (this.keepRunning)
                {
                    while (this.lineBuffer.Count > 0)
                    {
                        lock (this.lineBuffer)
                        {
                            this.logWriter.WriteLine(this.WriteToScreen(this.lineBuffer[0]));
                            this.lineBuffer.RemoveAt(0);
                        }
                    }
                    Thread.Sleep(50);
                }
                if (this.lineBuffer.Count <= 0)
                    return;
                for (int index = 0; index < this.lineBuffer.Count; ++index)
                    this.logWriter.WriteLine(this.WriteToScreen(this.lineBuffer[index]));
            }));
            this.logThread.Start();
        }

        public void WriteLog(string line)
        {
            lock (this.lineBuffer)
                this.lineBuffer.Add(line);
        }

        public void Close()
        {
            this.keepRunning = false;
            while (this.logThread.IsAlive)
                Thread.Sleep(100);
            this.logWriter.WriteLine(this.WriteToScreen("Log ended at " + DateTime.Now.ToLongTimeString()));
            this.logWriter.Close();
        }

        private string WriteToScreen(string text)
        {
            Console.WriteLine(text);
            return text;
        }
    }
}
