﻿// Copyright 2012 Moonfire Games
// Released under the MIT license
// http://mfgames.com/mfgames-writing/license

using System;
using System.Collections.Generic;

namespace MfGames.Writing.Cli
{
	/// <summary>
	/// Defines the entry point for the command-line interface for MfGames Writing.
	/// </summary>
	internal class CliEntry
	{
		/// <summary>
		/// Starting point for the application.
		/// </summary>
		/// <param name="args">The command-line arguments to the application. </param>
		private static void Main(string[] args)
		{
			// Parse the arguments and places it into the separate objects.
			CliArguments arguments = ParseArguments(args);

			// If we aren't doing "docbook" and "gather", we don't allow it.
			if (arguments.Format != "docbook")
			{
				throw new ApplicationException("The only format supported is 'docbook'.");
			}

			if (arguments.Operation != "gather")
			{
				throw new ApplicationException("The only operation supported is 'gather'.");
			}

			// Create the process arguments.
			var process = new DocbookGatherProcess();

			process.InputFilename = arguments.RemainingArguments[0];
			process.OutputFilename = arguments.RemainingArguments[1];

			process.Run();
		}

		/// <summary>
		/// Parses the command line arguments and places it into an appropriate object.
		/// </summary>
		/// <param name="args">The args.</param>
		private static CliArguments ParseArguments(IEnumerable<string> args)
		{
			// Create a new options argument.
			var arguments = new CliArguments();

			// Go through the arguments and populate the various fields.
			int ordinal = 0;

			foreach (string arg in args)
			{
				// If we are a command-line option, then populate those fields.
				if (arg.StartsWith("--"))
				{
					switch (arg.Substring(2))
					{
						case "root-directory":
						case "search-directory":
							break;
					}

					// We don't do any other processing of arguments.
					continue;
				}

				// For everything else, just place it in the ordinals.
				switch (ordinal++)
				{
					case 0:
						arguments.Format = arg;
						break;

					case 1:
						arguments.Operation = arg;
						break;

					default:
						arguments.RemainingArguments.Add(arg);
						break;
				}
			}

			// Return the resulting arguments object.
			return arguments;
		}
	}
}
