﻿#region License
//   Copyright 2010 John Sheehan
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 
#endregion

using System;
using RestSharp;
using RestSharp.Extensions;


namespace Twilio
{
	public partial class TwilioRestClient
	{
		/// <summary>
		/// Returns a list of Recordings, each representing a recording generated during the course of a phone call. 
		/// The list includes paging information.
		/// Makes a GET request to the Recordings List resource.
		/// </summary>
		public RecordingResult ListRecordings()
		{
			return ListRecordings(null, null, null, null);
		}

		/// <summary>
		/// Returns a filtered list of Recordings, each representing a recording generated during the course of a phone call. 
		/// The list includes paging information.
		/// Makes a GET request to the Recordings List resource.
		/// </summary>
		/// <param name="callSid">(Optional) The CallSid to retrieve recordings for</param>
		/// <param name="dateCreated">(Optional) The date the recording was created (GMT)</param>
		/// <param name="pageNumber">The page to start retrieving results from</param>
		/// <param name="count">How many results to retrieve</param>
		public RecordingResult ListRecordings(string callSid, DateTime? dateCreated, int? pageNumber, int? count)
		{
			var request = new RestRequest();
			request.Resource = "Accounts/{AccountSid}/Recordings";

			if (callSid.HasValue()) request.AddParameter("CallSid", callSid);
			if (dateCreated.HasValue) request.AddParameter("DateCreated", dateCreated.Value.ToString("yyyy-MM-dd"));
			if (pageNumber.HasValue) request.AddParameter("Page", pageNumber.Value);
			if (count.HasValue) request.AddParameter("PageSize", count.Value);

			return Execute<RecordingResult>(request);
		}

		/// <summary>
		/// Retrieve the details for the specified recording instance.
		/// Makes a GET request to a Recording Instance resource.
		/// </summary>
		/// <param name="recordingSid">The Sid of the recording to retrieve</param>
		public Recording GetRecording(string recordingSid)
		{
			var request = new RestRequest();
			request.Resource = "Accounts/{AccountSid}/Recordings/{RecordingSid}.xml";
			request.RootElement = "Recording";

			request.AddParameter("RecordingSid", recordingSid, ParameterType.UrlSegment);

			return Execute<Recording>(request);
		}

		/// <summary>
		/// Delete the specified recording instance. Makes a DELETE request to a Recording Instance resource.
		/// </summary>
		/// <param name="recordingSid">The Sid of the recording to delete</param>
		public DeleteStatus DeleteRecording(string recordingSid)
		{
			var request = new RestRequest(Method.DELETE);
			request.Resource = "Accounts/{AccountSid}/Recordings/{RecordingSid}.xml";
			request.RootElement = "Recording";

			request.AddParameter("RecordingSid", recordingSid, ParameterType.UrlSegment);

			var response = Execute(request);
			return response.StatusCode == System.Net.HttpStatusCode.NoContent ? DeleteStatus.Success : DeleteStatus.Failed;
		}

		/// <summary>
		/// Retrieves the transcription text for the specified recording, if it was transcribed. 
		/// Makes a GET request to a Recording Instance resource.
		/// </summary>
		/// <param name="recordingSid">The Sid of the recording to retreive the transcription for</param>
		public string GetRecordingText(string recordingSid)
		{
			var request = new RestRequest();
			request.Resource = "Accounts/{AccountSid}/Recordings/{RecordingSid}.txt";
			request.AddParameter("RecordingSid", recordingSid, ParameterType.UrlSegment);

			var response = Execute(request);
			return response.Content;
		}
	}
}