import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import NumberPredictComponent from './components/NumberPredictComponent';

import './custom.css'

export default () => (
    <Layout>
        <Route exact path='/' component={NumberPredictComponent} />
        <Route path='/mnist' component={NumberPredictComponent} />
    </Layout>
);
