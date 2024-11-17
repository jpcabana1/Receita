import React, { useEffect, useContext, createContext, useState } from 'react';
import { View, Text, TextInput, Button, StyleSheet, FlatList, ActivityIndicator, Modal, TouchableOpacity, Image } from 'react-native';

import LoginScreen from './components/LoginScreen'
import SearchScreen from './components/SearchScreen'

export const DataContext = createContext();
const App = () => {
    return (
        <DataProvider>
            <View style={styles.container}>
                <AuthenticationHandlerComponent />
            </View>
        </DataProvider>
    );
};

const AuthenticationHandlerComponent = () => {
    const { token } = useContext(DataContext);

    return (
        token !== "" ? <SearchScreen /> : <LoginScreen />
    );
}

export const DataProvider = ({ children }) => {
    const [token, setToken] = useState("");

    return (
        <DataContext.Provider value={{ token, setToken }}>
            {children}
        </DataContext.Provider>
    );
};


export default App;
