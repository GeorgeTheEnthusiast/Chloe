﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Flights.Client.Domain
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FlightsEntities : DbContext
    {
        public FlightsEntities()
            : base("name=FlightsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Carriers> Carriers { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Currencies> Currencies { get; set; }
        public virtual DbSet<Flights> Flights { get; set; }
        public virtual DbSet<Net> Net { get; set; }
        public virtual DbSet<NotificationReceivers> NotificationReceivers { get; set; }
        public virtual DbSet<NotificationReceiversGroups> NotificationReceiversGroups { get; set; }
        public virtual DbSet<ReceiverGroups> ReceiverGroups { get; set; }
        public virtual DbSet<SearchCriterias> SearchCriterias { get; set; }
    }
}
