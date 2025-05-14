using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour {
    public string locationName;

    [TextArea]
    public string description;
    
    public Connection[] connections;

    public List<Item> items = new List<Item>();

    public string GetItemsText() {
        if (items.Count == 0) return "";

        string result = "\nYou see ";
        bool first = true;
        foreach(Item item in items) {
            if (item.itemEnabled) {
                if (!first) {
                    result += " and ";
                }
                first = false;
                result += item.description;
            }
        }
        if (result == "\nYou see ") {
            return"";
        }
        result += "\n";
        return result;
    }

    public string GetConnectionsText() {
        string result = "\n";
        foreach(Connection connection in connections) {
            if (connection.connectionEnabled) {
                result += "<color=orange>" + connection.description + "</color>\n";
            }
        }
        return result;
    }

    public Connection GetConnection(string connectionNoun) {
        foreach(Connection connection in connections) {
            if (connection.connectionName.ToLower() == connectionNoun.ToLower()) {
                return connection;
            }
        }
        return null;
    }

    internal bool HasItem(Item itemToCheck) {
        foreach(Item item in items) {
            if (item == itemToCheck && item.itemEnabled) {
                return true;
            }
        }
        return false;
    }
}
