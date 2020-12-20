import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import NumberPredictComponent from './components/NumberPrediction/NumberPredictComponent';
import CatsAndDogsComponent from './components/CatsAndDogs/CatsAndDogsComponent';

import './custom.css'

export default () => (
    <Layout>
        <Route exact path='/' component={NumberPredictComponent} />
        <Route path='/mnist' component={NumberPredictComponent} />
        <Route path='/cats_and_dogs' component={CatsAndDogsComponent} />
    </Layout>
);
