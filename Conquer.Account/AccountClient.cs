using Microsoft.AspNetCore.Connections;

namespace Conquer.Account;

public class AccountClient(ConnectionContext connection) : Client(connection);