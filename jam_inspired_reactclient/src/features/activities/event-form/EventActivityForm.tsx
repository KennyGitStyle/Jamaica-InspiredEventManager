import React, { useState, useContext, useEffect } from 'react';
import { Segment, Form, Button, Grid } from 'semantic-ui-react';
import { EventActivityFormValues } from '../../../app/models/event-activity';
import { v4 as uuid } from 'uuid';
import { observer } from 'mobx-react-lite';
import { RouteComponentProps } from 'react-router';
import { Form as FinalForm, Field } from 'react-final-form';
import TextInput from '../../../app/common/form/TextInput';
import TextAreaInput from '../../../app/common/form/TextAreaInput';
import SelectInput from '../../../app/common/form/SelectInput';
import { category } from '../../../app/common/category-options/CategoryOptions';
import DateInput from '../../../app/common/form/DateInput';
import { combineDateAndTime } from '../../../app/common/util/Util';
import { combineValidators, isRequired, composeValidators, hasLengthGreaterThan } from 'revalidate';
import { RootStoreContext } from '../../../app/stores/rootStore';



const validate = combineValidators({

  title: isRequired({ message: 'Title is required' }),
  category: isRequired({ message: 'Category is required' }),
  description: composeValidators(
      isRequired('Description'),
      hasLengthGreaterThan(6)({ message: 'requires minimum of 6 char' })
  )(),
  city: isRequired({ message: 'City is required' }),
  venue: isRequired({ message: 'Venue is requred' }),
  date: isRequired({ message: 'Date is required' }),
  time: isRequired({ message: 'Time is required' })
})

interface DetailParams {
  id: string;
}

const EventActivityForm: React.FC<RouteComponentProps<DetailParams>> = ({ match, history }) => {
  const rootStore = useContext(RootStoreContext);
  const { createEventActivity, editEventActivity, submitting, loadEventActivity} = rootStore.eventActivityStore;

  const [activity, setActivity] = useState(new EventActivityFormValues());
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (match.params.id) {
      setLoading(true)
      loadEventActivity(match.params.id).then(
        (activity) => setActivity(new EventActivityFormValues(activity)))
        .finally(() => setLoading(false))
    }

  }, [loadEventActivity, match.params.id]);

  const handleFinalFormSubmit = (values: any) => {
    const dateAndTime = combineDateAndTime(values.date, values.time)
    const { date, time, ...activity } = values;
    activity.date = dateAndTime;

    if (!activity.id) {
      let newActivity = {
        ...activity,
        id: uuid()
      }
      createEventActivity(newActivity)
    } else {
      editEventActivity(activity)
    }
  }

  return (
    <Grid>
      <Grid.Column width={10}>
        <Segment clearing>
          <FinalForm initialValues={activity} validate={validate} onSubmit={handleFinalFormSubmit} render={({ handleSubmit, invalid, pristine }) => {
            return (
              <Form onSubmit={handleSubmit} loading={loading}>
                <Field name='title' placeholder='Title' value={activity.title} component={TextInput} />
                <Field name='description' rows={3} placeholder='Description' value={activity.description} component={TextAreaInput} />
                <Field component={SelectInput} options={category} name='category' placeholder='Category' value={activity.category} />
                <Form.Group widths='equal'>
                  <Field name='date' placeholder='Date' date={true} value={activity.date} component={DateInput} />
                  <Field name='time' placeholder='Time' time={true} value={activity.time} component={DateInput} />
                </Form.Group>
                <Field name='city' placeholder='City' value={activity.city} component={TextInput} />
                <Field name='venue' placeholder='Venue' value={activity.venue} component={TextInput} />
                <Button loading={submitting} floated='right' disabled={loading || invalid || pristine} positive type='submit' content='Submit' />
                <Button onClick={activity.id
                  ? () => history.push(`/events/${activity.id}`)
                  : () => history.push('/events')
                }
                  disabled={loading} floated='right' color='black' type='button' content='Cancel' />
              </Form>
            )
          }} />

        </Segment>
      </Grid.Column>
    </Grid>

  );
};

export default observer(EventActivityForm);
