// =====================================================================
//
//  This file is part of the Microsoft Dynamics CRM SDK code samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  This source code is intended only as a supplement to Microsoft
//  Development Tools and/or on-line documentation.  See these other
//  materials for detailed information regarding Microsoft code samples.
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
//
// =====================================================================
//<snippetCreateConnection>
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
// These namespaces are found in the Microsoft.Xrm.Sdk.dll assembly
// found in the SDK\bin folder.
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
// This namespace is found in Microsoft.Crm.Sdk.Proxy.dll assembly
// found in the SDK\bin folder.
using SbrinnaCoreFramework.Sdk.Xrm;

namespace SbrinnaCoreFramework.Sdk.Helpers
{
    /// <summary>
    /// This sample shows how to create a connection between two entities.
    /// </summary>
    public class CreateConnection
    {
        #region Class Level Members
        
        /// <summary>
        /// Stores the organization service proxy.
        /// </summary>
        OrganizationServiceProxy _serviceProxy;

        // Define the IDs needed for this sample.
        public Guid _connectionRoleId;
        public Guid _connectionId;
        public Guid _contactId1;
        public Guid _contactId2;

        #endregion Class Level Members

        #region How To Sample Code
        /*/// <summary>
        /// Create and configure the organization service proxy.
        /// Call the method to create any data that this sample requires.
        /// Create a new connection between the account and the contact.
        /// Optionally delete any entity records that were created for this sample.
        /// </summary>
                /// <param name="serverConfig">Contains server connection information.</param>
        /// <param name="promptforDelete">When True, the user will be prompted to delete all
        /// created entities.</param>
        public void Run(ServerConnection.Configuration serverConfig, bool promptForDelete)
        {
            try
            {
                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = new OrganizationServiceProxy(serverConfig.OrganizationUri,
                                                                    serverConfig.HomeRealmUri,
                                                                    serverConfig.Credentials,
                                                                    serverConfig.DeviceCredentials))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.ServiceConfiguration.CurrentServiceEndpoint.Behaviors.Add(new ProxyTypesBehavior());


                    // Call the method to create any data that this sample requires.
                    CreateRequiredRecords();

                    //<snippetCreateConnection1>
                    // Create a connection between the account and the contact.
                    Connection newConnection = new Connection
                    {
                        Record1Id = new EntityReference(Contact.EntityLogicalName,
                            _contactId1),
                        Record1RoleId = new EntityReference(ConnectionRole.EntityLogicalName,
                            _connectionRoleId),
                        Record2Id = new EntityReference(Contact.EntityLogicalName,
                            _contactId2)
                    };
                    
                    _connectionId = _serviceProxy.Create(newConnection);
                    //</snippetCreateConnection1>  

                    Console.WriteLine(
                        "Created a connection between the account and the contact.");

                    DeleteRequiredRecords(promptForDelete);

                }
            }
            // Catch any service fault exceptions that Microsoft Dynamics CRM throws.
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>)
            {
                // You can handle an exception here or pass it back to the calling method.
                throw;
            }
        }*/

        /*/// <summary>
        /// This method creates any entity records that this sample requires.
        /// Create a new connectionrole instance. 
        /// Create related Connection Role Object Type Code records
        /// for the account and the contact entities.
        /// </summary>
        public void CreateRequiredRecords()
        {
            // Define some anonymous types to define the range 
            // of possible connection property values.
            var Categories = new
            {
                Business = 1,
                Family = 2,
                Social = 3,
                Sales = 4,
                Other = 5
            };

            // Create a Connection Role for account and contact
            ConnectionRole newConnectionRole = new ConnectionRole
            {
                Name = "Example Connection Role",
                Category = new OptionSetValue(Categories.Business)
            };

            _connectionRoleId = _serviceProxy.Create(newConnectionRole);
            Console.WriteLine("Created {0}.", newConnectionRole.Name);

            // Create a related Connection Role Object Type Code record for Account
            ConnectionRoleObjectTypeCode newAccountConnectionRoleTypeCode
                = new ConnectionRoleObjectTypeCode
                {
                    ConnectionRoleId = new EntityReference(
                        ConnectionRole.EntityLogicalName, _connectionRoleId),
                    AssociatedObjectTypeCode = Account.EntityLogicalName
                };

            _serviceProxy.Create(newAccountConnectionRoleTypeCode);
            Console.WriteLine(
                "Created a related Connection Role Object Type Code record for Account."
                );

            // Create a related Connection Role Object Type Code record for Contact
            ConnectionRoleObjectTypeCode newContactConnectionRoleTypeCode
                = new ConnectionRoleObjectTypeCode
                {
                    ConnectionRoleId = new EntityReference(
                        ConnectionRole.EntityLogicalName, _connectionRoleId),
                    AssociatedObjectTypeCode = Contact.EntityLogicalName
                };

            _serviceProxy.Create(newContactConnectionRoleTypeCode);
            Console.WriteLine(
                "Created a related Connection Role Object Type Code record for Contact."
                );

            // Create an Account
            Contact setupAccount = new Contact { FirstName = "Example Account" };
            _contactId1 = _serviceProxy.Create(setupAccount);
            Console.WriteLine("Created {0}.", setupAccount.FirstName);

            // Create a Contact
            Contact setupContact = new Contact { LastName = "Example Contact" };
            _contactId2 = _serviceProxy.Create(setupContact);
            Console.WriteLine("Created {0}.", setupContact.LastName);

            return;
        }*/

        /*/// <summary>
        /// Deletes any entity records that were created for this sample.
        /// <param name="prompt">Indicates whether to prompt the user 
        /// to delete the records created in this sample.</param>
        /// </summary>
        public void DeleteRequiredRecords(bool prompt)
        {
            bool deleteRecords = true;

            if (prompt)
            {
                Console.WriteLine("\nDo you want these entity records deleted? (y/n)");
                String answer = Console.ReadLine();

                deleteRecords = (answer.StartsWith("y") || answer.StartsWith("Y"));
            }

            if (deleteRecords)
            {
                _serviceProxy.Delete(Connection.EntityLogicalName, _connectionId);
                _serviceProxy.Delete(Contact.EntityLogicalName, _contactId1);
                _serviceProxy.Delete(Contact.EntityLogicalName, _contactId2);
                _serviceProxy.Delete(ConnectionRole.EntityLogicalName, _connectionRoleId);

                Console.WriteLine("Entity records have been deleted.");
            }
        }*/

        #endregion How To Sample Code         
    }
}
//</snippetCreateConnection>
