import React, { useEffect, useContext, createContext, useState } from 'react';
import { View, Text, TextInput, Button, StyleSheet, FlatList, ActivityIndicator, Modal, TouchableOpacity, Image } from 'react-native';
import { styles } from '../assets/styles/styles'
import {login} from '../services/loginservice'
import { host } from '../util/variables';

const LoginScreen = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const { setToken } = useContext(DataContext);

  const handleLogin = () => {
      fetch(`${host}/login`, {
          method: "POST",
          headers: {
              "accept": "*/*",
              "Content-Type": "application/json"
          },
          body: JSON.stringify({
              "username": username,
              "password": password
          }),
          redirect: "follow"
      })
          .then(res => res.json())
          .then(result => setToken(result.token))
          .catch(error => alert(`Credenciais inválidas`))

  };

  return (
      <View style={styles.container}>
          <Text style={styles.title}>Login</Text>
          <TextInput
              style={styles.input}
              placeholder="Usuário"
              value={username}
              onChangeText={setUsername}
          />
          <TextInput
              style={styles.input}
              placeholder="Senha"
              secureTextEntry
              value={password}
              onChangeText={setPassword}
          />
          <Button title="Entrar" onPress={handleLogin} />
      </View>
  );
};

export default LoginScreen;