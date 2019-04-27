﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace OGN.FlightLog.Client.Models
{
    public class Flight
    {
        public static string Header = "DATE,SEQ_NR,ID,CALLSIGN,COMPETITION_NUMBER,TYPE,DETAILED_TYPE,CREW1,CREW2,TKOF_TIME,TKOF_AP,TKOF_RWY,RESERVED,LDG_TIME,LDG_AP,LDG_RWY,LDG_TURN,MAX_ALT,AVERAGE_CLIMB_RATE,FLIGHT_TIME,DAY_DIFFERENCE,LAUNCH_METHOD,INITIAL_CLIMBRATE,TOW_ID,TOW_CALLSIGN,TOW_COMPETITION_NUMBER,TOW_SEQUENCE_NUMBER";
        public enum Columns
        {
            DATE,SEQ_NR,ID,CALLSIGN,COMPETITION_NUMBER,TYPE,DETAILED_TYPE,CREW1,CREW2,TKOF_TIME,TKOF_AP,TKOF_RWY,RESERVED,LDG_TIME,LDG_AP,LDG_RWY,LDG_TURN,MAX_ALT,AVERAGE_CLIMB_RATE,FLIGHT_TIME,DAY_DIFFERENCE,LAUNCH_METHOD,INITIAL_CLIMBRATE,TOW_ID,TOW_CALLSIGN,TOW_COMPETITION_NUMBER,TOW_SEQUENCE_NUMBER
        }

        public Flight()
        {
            // Used only when entity framework initializes objects
            State = EntityState.Unchanged;
        }

        public Flight(Client.Options options, int row)
        {
            // E.g. "EHDL30052015QFEz2m"
            this.dataset = options.GetDatasetIdentifier();
            this.row = row;

            // Flight id based on parameters + row in datasource e.g. "EHDL30052015z2metric" + "1"
            this.ID = this.dataset + this.row.ToString();

            // FROM options
            //   "date": "30052015",
            //   "airfield": "EHDL",
            //   "unit": "metric",
            this.Date = options.Date.Date;
            this.airfield = options.AirfieldParameter;
            this.unit = options.UnitParameter;
            this.timezone = options.TimeZoneParameter;
        }

        public Flight(Client.Options options, int row, string line) : this(options, row)
        {
            //"DATE,SEQ_NR,ID,CALLSIGN,COMPETITION_NUMBER,TYPE,
            // DETAILED_TYPE,CREW1,CREW2,TKOF_TIME,TKOF_AP,TKOF_RWY,RESERVED,
            // LDG_TIME,LDG_AP,LDG_RWY,LDG_TURN,
            // MAX_ALT,AVERAGE_CLIMB_RATE,
            // FLIGHT_TIME,DAY_DIFFERENCE,LAUNCH_METHOD,INITIAL_CLIMBRATE,TOW_ID,TOW_CALLSIGN,TOW_COMPETITION_NUMBER,TOW_SEQUENCE_NUMBER"

            string[] data = line.Split(',');
            this.seq_nr = data[(int)Columns.SEQ_NR];
            this.identifier = data[(int)Columns.ID];
            this.callsign = data[(int)Columns.CALLSIGN];
            this.competition_number = data[(int)Columns.COMPETITION_NUMBER];
            this.plane_type = data[(int)Columns.TYPE];
            this.detailed_plane_type = data[(int)Columns.DETAILED_TYPE];
            this.crew1 = data[(int)Columns.CREW1];
            this.crew2 = data[(int)Columns.CREW2];
            this.tkof_time = Parse.DateTimeOffset(data[(int)Columns.TKOF_TIME], options.TimeZone);
            this.tkof_ap = data[(int)Columns.TKOF_AP];
            this.tkof_rwy = Parse.Integer(data[(int)Columns.TKOF_RWY]);
            this.ldg_time = Parse.DateTimeOffset(data[(int)Columns.LDG_TIME], options.TimeZone);
            this.ldg_ap = data[(int)Columns.LDG_AP];
            this.ldg_rwy = Parse.Integer(data[(int)Columns.LDG_RWY]);
            this.ldg_turn = Parse.Decimal(data[(int)Columns.LDG_TURN]);
            this.max_alt = Parse.Integer(data[(int)Columns.MAX_ALT]);
            this.average_climb_rate = Parse.Decimal(data[(int)Columns.AVERAGE_CLIMB_RATE]);
            this.flight_time = Parse.TimeSpan(data[(int)Columns.FLIGHT_TIME]);
            this.day_difference = Parse.Integer(data[(int)Columns.DAY_DIFFERENCE]);
            this.launch_method = data[(int)Columns.LAUNCH_METHOD];
            this.initial_climbrate = Parse.Decimal(data[(int)Columns.INITIAL_CLIMBRATE]);
            this.tow_identifier = data[(int)Columns.TOW_ID];
            this.tow_callsign = data[(int)Columns.TOW_CALLSIGN];
            this.tow_competition_number = data[(int)Columns.TOW_COMPETITION_NUMBER];
            this.tow_sequence_number = data[(int)Columns.TOW_SEQUENCE_NUMBER]; 
        }

        internal class Parse
        {
            /// <summary>
            /// Handling incomming formats of "" or "11:26:04" and taking care of adding the timeZone information into the entry
            /// </summary>
            /// <param name="value"></param>
            /// <param name="timeZone"></param>
            /// <returns></returns>
            internal static DateTimeOffset? DateTimeOffset(string value, int timeZone)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return null;

                TimeSpan time;
                if (!System.TimeSpan.TryParse(value, out time))
                    return null;

                return new DateTimeOffset(time.Ticks, new TimeSpan(timeZone, 0, 0));
            }

            /// <summary>
            /// Handling incomming formats of "" or "00:05" for hours and minutes
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            internal static TimeSpan? TimeSpan(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return null;

                TimeSpan time;
                if (!System.TimeSpan.TryParseExact(value, @"hh:mm", null, out time))
                    return null;

                return time;
            }

            internal static Decimal? Decimal(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return null;

                if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal numeric))
                    return numeric;

                return null;
            }

            internal static int? Integer(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return null;

                if (int.TryParse(value, out int numeric))
                    return numeric;

                return null;
            }
        }

        public string dataset { get; set; }
        public int row { get; set; }

        [Key]
        public string ID { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string airfield { get; set; }
        public string unit { get; set; }
        public string timezone { get; set; }

        private string seq_nr;

        public string identifier { get; set; }
        public string callsign { get; set; }

        private string competition_number;
        private string plane_type;
        private string detailed_plane_type;
        private string crew1;
        private string crew2;
        private DateTimeOffset? tkof_time;
        private string tkof_ap;
        private int? tkof_rwy;
        private DateTimeOffset? ldg_time;
        private string ldg_ap;
        private int? ldg_rwy;
        private decimal? ldg_turn;
        private int? max_alt;
        private decimal? average_climb_rate;
        private TimeSpan? flight_time;
        private int? day_difference;
        private string launch_method;
        private decimal? initial_climbrate;
        private string tow_identifier;
        private string tow_callsign;
        private string tow_competition_number;
        private string tow_sequence_number;

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public EntityState State { get; set; }
    }
}
