import React from 'react';
import { Item, Button, Segment, Icon } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import { IEventActivity } from '../../../app/models/event-activity';
import { observer } from 'mobx-react-lite';
import {format} from 'date-fns';

const EventActivityListItem: React.FC<{ event: IEventActivity }> = ({ event }) => {
  return (
    <Segment.Group>
      <Segment>
        <Item.Group>
          <Item>
            <Item.Image size='tiny' rounded src='/assets/user.png' />
            <Item.Content>
              <Item.Header as='a'>{event.title}</Item.Header>
              <Item.Description verticalAlign='bottom'>
                Hosted by Ken
            </Item.Description>
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>
      <Segment>
        <Icon name='clock' /> {format(event.date, 'h:mm a')}
        <Icon name='marker' /> {event.venue}, {event.city}
      </Segment>
      <Segment secondary>
        Attendees section
      </Segment>
      <Segment clearing>
        <span>{event.description}</span>
        <Button
          as={Link} to={`/events/${event.id}`}
          floated='right'
          content='View'
          color='green'
        />
      </Segment>
    </Segment.Group>

  )
}

export default observer(EventActivityListItem);